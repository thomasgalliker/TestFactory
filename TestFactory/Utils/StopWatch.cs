using System;
using System.Diagnostics;

namespace TestFactory.Utils
{
    public class StopWatch : IDisposable
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly Action<TimeSpan> callback;

        public StopWatch()
        {
            this.stopwatch.Start();
        }

        public StopWatch(Action<TimeSpan> callback) : this()
        {
            this.callback = callback;
        }

        public static StopWatch Start(Action<TimeSpan> callback)
        {
            return new StopWatch(callback);
        }

        public void Dispose()
        {
            this.stopwatch.Stop();
            this.callback?.Invoke(this.Result);
        }

        public TimeSpan Result => this.stopwatch.Elapsed;
    }
}