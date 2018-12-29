using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TestFactory
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ParallelTestStep : TestStepBase
    {
        public IEnumerable<ITestStep> TestSteps { get; }

        public ParallelTestStep(IEnumerable<ITestStep> parallelTestSteps, string name = null)
            : base(name)
        {
            this.TestSteps = parallelTestSteps;
        }

        public override async Task<ITestStepResult> Run()
        {
            var parallelTestStep = this;
            var stopwatch = Stopwatch.StartNew();
            AggregateException aggregateException = null;
            var testStepResults = new ITestStepResult[0];

            var allTasks = Task.WhenAll(parallelTestStep.TestSteps.Select(testStep => testStep.Run()));
            try
            {
                testStepResults = await allTasks;
            }
            catch
            {
                aggregateException = allTasks.Exception;
            }

            stopwatch.Stop();
            return new ParallelTestStepResult(this, stopwatch.Elapsed, aggregateException == null, testStepResults, aggregateException);
        }
    }
}
