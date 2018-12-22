
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestFactory
{
    public class WaitStep : TestStepBase
    {
        private readonly TimeSpan timeSpan;

        public WaitStep(TimeSpan timeSpan, string name = null) : base(name)
        {
            this.timeSpan = timeSpan;
        }

        public WaitStep(int millisecondsTimeout, string name = null) : this(TimeSpan.FromMilliseconds(millisecondsTimeout), name)
        {
        }

        public override Task<ITestStepResult> Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var totalMilliseconds = (int)this.timeSpan.TotalMilliseconds;

#if NETSTANDARD
            System.Threading.Tasks.Task.Delay(totalMilliseconds).Wait();
#else
            System.Threading.Thread.Sleep(totalMilliseconds);
#endif

            stopwatch.Stop();
            return Task.FromResult<ITestStepResult>(new TestStepResult(
                testStep: this,
                duration: stopwatch.Elapsed,
                isSuccessful: true));
        }
    }
}