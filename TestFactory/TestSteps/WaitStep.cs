
using System;
using System.Diagnostics;

namespace TestFactory.TestSteps
{
    public class WaitStep : ITestStep
    {
        private readonly TimeSpan timeSpan;

        public WaitStep(TimeSpan timeSpan)
        {
            this.timeSpan = timeSpan;
        }

        public WaitStep(int millisecondsTimeout)
        {
            this.timeSpan = TimeSpan.FromMilliseconds(millisecondsTimeout);
        }

        public ITestStepResult Run()
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
            return new TestStepResult(
                testStep: this,
                duration: stopwatch.Elapsed,
                isSuccessful: true);
        }
    }
}