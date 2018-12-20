using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TestFactory.Extensions;

namespace TestFactory.TestSteps
{
    public class FuncTestStep<TOut> : ITestStep
    {
        private readonly Func<TOut> func;

        public FuncTestStep(Func<TOut> func, string name = null)
        {
            this.func = func;
            this.Name = name ?? this.GetType().GetFormattedName();
        }

        public string Name { get; }

        public Task<ITestStepResult> Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = default(TOut);
            Exception exception = null;
            try
            {
                result = this.func();
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
                result: result,
                exception: exception));
        }
    }
}