using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestFactory
{
    public class TaskTestStep : ITestStep
    {
        private readonly Func<Task> taskFunc;

        public TaskTestStep(Func<Task> taskFunc)
        {
            this.taskFunc = taskFunc;
        }

        public async Task<ITestStepResult> Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Exception exception = null;
            try
            {
                var task = this.taskFunc();
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            stopwatch.Stop();
            return new TestStepResult(this, stopwatch.Elapsed, exception == null, exception);
        }
    }
}