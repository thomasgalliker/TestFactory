
using System;

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
#if NETSTANDARD
            System.Threading.Tasks.Task.Delay(this.timeSpan.Milliseconds).Wait();
#else
            System.Threading.Thread.Sleep(this.timeSpan.Milliseconds);
#endif
            return new TestStepResult(this);
        }
    }
}