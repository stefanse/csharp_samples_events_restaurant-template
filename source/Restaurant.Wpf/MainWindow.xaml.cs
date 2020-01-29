using Restaurant.Core;
using System;
using System.Text;

namespace Restaurant.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        readonly DateTime _startTime = DateTime.Parse("12:00");
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Initialized(object sender, EventArgs e)
        {
            FastClock.Instance.Time = _startTime;
            FastClock.Instance.OneMinuteIsOver += Instance_OneMinuteIsOver;
            FastClock.Instance.IsRunning = true;
            Waiter waiter = new Waiter(_startTime);
            waiter.TaskFinished += OnTaskFinished;
            
        }
        private void OnTaskFinished(Object sender, string message)
        {
            StringBuilder text = new StringBuilder(TextBlockLog.Text);
            text.Append(FastClock.Instance.Time.ToShortTimeString() + "\t");
            text.Append(message + "\n");
            TextBlockLog.Text = text.ToString();
        }

        private void Instance_OneMinuteIsOver(object sender, DateTime e)
        {
            Title = $"Restaurantsimulator, {e.ToShortTimeString()}";
        }
    }
}
