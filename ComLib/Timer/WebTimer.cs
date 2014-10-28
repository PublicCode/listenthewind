using System.Timers;

namespace ComLib.Timer
{
    public abstract class WebTimer
    {
        // TODO: We definitely need some logging framework.
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval">The time in milliseconds between two invocations of Task().</param>
        protected WebTimer(double interval)
        {
            _timer.Interval = interval;
            _timer.Elapsed += NoisyTask;
        }

        // public or protected?
        public void Start()
        {
            _timer.Start();
        }

        // public or protected?
        public void Stop()
        {
            _timer.Stop();
        }

        public double Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        private void NoisyTask(object sender, ElapsedEventArgs e)
        {
            Stop();
            Task();
            Start();
        }

        public abstract void Task();
    }
}
