using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Utils;

namespace Restaurant.Core
{
    public class Waiter
    {
        const string ArticlesFilename = "articles.csv";
        const string TasksFilename = "tasks.csv";
        readonly List<Task> _tasks;
        readonly Dictionary<string, Guest> _guests;

        private DateTime _startTime;
        public event EventHandler<string> TaskFinished;

        public Waiter(DateTime startTime)
        {
            var articles = ReadArticlesFromCsv();
            _guests = new Dictionary<string, Guest>();
            _tasks = ReadTasksFromCsv(_guests, articles);
            FastClock.Instance.OneMinuteIsOver += Instance_OneMinuteIsOver;
            _startTime = startTime;
        }

        private void Instance_OneMinuteIsOver(object sender, DateTime e)
        {
            DoTasks();
        }


        /// <summary>
        /// Kellner führt Task aus, falls etwas zu tun ist.
        /// </summary>
        public void DoTasks()
        {
            //Gibt es was zu tun?
            while (_tasks.Count > 0
                   && _startTime.AddMinutes(_tasks.ElementAt(0).Delay) <= FastClock.Instance.Time)
            {
                Task task = _tasks.ElementAt(0);
                //Tasks ausführen
                _tasks.RemoveAt(0);
                string message = "";
                switch (task.TaskType)
                {

                    case TaskType.Order:
                        task.Guest.OrderArticle(task.Article);
                        Task deliverTask = new Task(TaskType.Deliver, task.Delay + task.Article.TimeToBuild,
                            task.Guest, task.Article);
                        _tasks.Add(deliverTask);
                        _tasks.Sort(); // damit die Auslieferung entsprechend der Zeit eingefügt wird
                        Debug.WriteLine(task);
                        message = task.ToString();
                        break;
                    case TaskType.Deliver:
                        Debug.WriteLine(task);
                        message = task.ToString();
                        break;
                    case TaskType.Pay:
                        double sum = task.Guest.CalculateSumOfOrders();
                        message = $"Pay\t{task.Guest.Name} \t {sum:f2}";
                        Debug.WriteLine(message);
                        _guests.Remove(task.Guest.Name);
                        break;
                }
                TaskFinished?.Invoke(this, message);
            }
            //// Rekursiver Aufruf der Taskbearbeitung bis in dieser Minute keine Aufträge anstehen.
            //DoTasks();
        }


        /// <summary>
        /// Tasks von csv-Datei in die Taskliste einlesen. Auch die Liste der Gäste
        /// mit den Gästen der Taskliste füllen.
        /// </summary>
        /// <param name="guests"></param>
        /// <param name="articles"></param>
        /// <returns>Taskliste</returns>
        private static List<Task> ReadTasksFromCsv(Dictionary<string, Guest> guests, Dictionary<string, Article> articles)
        {
            string[] lines = File.ReadAllLines(
                MyFile.GetFullNameInApplicationTree(TasksFilename),
                Encoding.Default);

            var tasks = new List<Task>();
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Split(';');
                string guestName = line[1];
                string articleName = line[3];
                TaskType taskType = (TaskType)Enum.Parse(typeof(TaskType), line[2]);
                int delay = int.Parse(line[0]);
                if (!guests.ContainsKey(guestName))
                {
                    guests.Add(guestName, new Guest(guestName));
                }
                // Gast ist genau einmal im Dictionary(Map)
                Guest guest = guests[guestName];
                //bool ok = guests.TryGetValue(guestName, out guest)
                Task task;
                if (string.IsNullOrEmpty(articleName))
                {
                    task = new Task(taskType, delay, guest);
                }
                else
                {
                    task = new Task(taskType, delay, guest, articles[articleName]);
                }
                tasks.Add(task);
            }
            return tasks;
        }

        /// <summary>
        /// Artikel aus der csv-Datei in die Artikel-Map einlesen.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, Article> ReadArticlesFromCsv()
        {

            string[] lines = File.ReadAllLines(path:
                MyFile.GetFullNameInApplicationTree(ArticlesFilename),
                Encoding.Default);
            var articles = new Dictionary<string, Article>();
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Split(';');
                Article article = new Article(line[0], double.Parse(line[1]), int.Parse(line[2]));
                articles.Add(line[0], article);

            }
            return articles;
        }

    }

}
