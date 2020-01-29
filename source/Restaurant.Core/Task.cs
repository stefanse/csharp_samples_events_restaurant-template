using System;

namespace Restaurant.Core
{
    public class Task : IComparable<Task>
    {
        public Task(TaskType taskType, int delay, Guest guest)
        {
            TaskType = taskType;
            Delay = delay;
            Guest = guest;
        }

        public Task(TaskType taskType, int delay, Guest guest, Article article) : this(taskType, delay, guest)
        {
            Article = article;
        }

        public TaskType TaskType { get; }

        public int Delay { get; }

        public Guest Guest { get; }

        public Article Article { get; }

        public int CompareTo(Task other)
        {
            return Delay.CompareTo(other.Delay);
        }

        public override string ToString()
        {
            return $"{TaskType}\t{Guest.Name}\t{Article.Name}";
        }
    }
}
