using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TestFactory.Extensions;

namespace TestFactory
{
    public class TaskTestStep : TestStepBase
    {
        private readonly Func<Task> createTask;

        public TaskTestStep(Func<Task> createTask, string name = null) : base(name)
        {
            this.createTask = createTask;
        }

        public override async Task<ITestStepResult> Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Exception exception = null;
            try
            {
                var task = this.createTask();
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
