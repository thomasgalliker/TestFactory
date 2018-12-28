using System;
using System.Diagnostics;

namespace TestFactory.Utils
{
    public class StopWatch : IDisposable
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly Action<TimeSpan> callback;

        public StopWatch(Action<TimeSpan> callback) : this(autostart: false, callback: callback)
        {
        }

        public StopWatch(bool autostart = false, Action<TimeSpan> callback = null)
        {
            this.callback = callback;
            if (autostart)
            {
                stopwatch.Start();
            }
        }

        public static StopWatch StartNew(Action<TimeSpan> callback)
        {
            return new StopWatch(callback).Start();
        }

        public static StopWatch StartNew()
        {
            return new StopWatch().Start();
        }

        public StopWatch Start()
        {
            stopwatch.Start();
            return this;
        }

        public StopWatch Stop()
        {
            stopwatch.Stop();
            callback?.Invoke(Elapsed);
            return this;
        }

        public void Dispose()
        {
            this.Stop();
        }

        public TimeSpan Elapsed => stopwatch.Elapsed;
    }
}
