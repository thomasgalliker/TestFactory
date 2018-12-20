using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestFactory.TestSteps
{
    public class ActionTestStep : ITestStep
    {
        private readonly Action action;

        public ActionTestStep(Action action, string name = nameof(ActionTestStep))
        {
            this.Name = name;
            this.action = action;
        }

        public string Name { get; }

        public Task<ITestStepResult> Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Exception exception = null;
            try
            {
                this.action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            stopwatch.Stop();
            return Task.FromResult<ITestStepResult>(new TestStepResult(
                testStep: this,
                duration: stopwatch.Elapsed,
                isSuccessful: exception == null,
                exception: exception));
        }
    }
}