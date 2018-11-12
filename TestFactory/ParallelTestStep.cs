using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TestFactory
{
    [DebuggerDisplay("ParallelTestStep: {this.TestSteps}")]
    public class ParallelTestStep : ITestStep
    {
        public IEnumerable<TaskTestStep> TestSteps { get; }

        public ParallelTestStep(IEnumerable<TaskTestStep> parallelTestSteps)
        {
            this.TestSteps = parallelTestSteps;
        }

        public async Task<ITestStepResult> Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Exception exception = null;
            ITestStepResult[] testStepResults = null;
            try
            {
                testStepResults = await Task.WhenAll(this.TestSteps.Select(ts => ts.Run()));
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            stopwatch.Stop();
            return new ParallelTestStepResult(this, stopwatch.Elapsed, exception == null, testStepResults, exception);
        }
    }
}