using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestFactory
{
    public class TaskTestStep : ITestStep
    {
        private readonly Func<Task> createTask;

        public TaskTestStep(Func<Task> createTask, string name = nameof(TaskTestStep))
        {
            this.createTask = createTask;
            this.Name = name;
        }

        public string Name { get; }

        public async Task<ITestStepResult> Run()
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