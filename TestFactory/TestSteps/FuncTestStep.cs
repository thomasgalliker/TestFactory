using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestFactory
{
    public class FuncTestStep<TOut> : TestStepBase
    {
        private readonly Func<TOut> func;

        public FuncTestStep(Func<TOut> func, string name = null) : base(name)
        {
            this.func = func;
        }

        public override Task<ITestStepResult> Run()
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