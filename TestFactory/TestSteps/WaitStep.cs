
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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

        public Task<ITestStepResult> Run()
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