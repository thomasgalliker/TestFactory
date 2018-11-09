using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestFactory
{
    public class SystemTestBuilder
    {
        private readonly List<ITestStep> testSteps;

        public SystemTestBuilder()
        {
            this.testSteps = new List<ITestStep>();
        }

        public SystemTestBuilder AddTestStep(ITestStep testStep)
        {
            this.testSteps.Add(testStep);

            return this;
        }

        public async Task<ITestResult> Run()
        {
            var testStepResults = new List<ITestStepResult>();
            foreach (var testStep in this.testSteps)
            {
                var stopwatch = new Stopwatch();
                
                try
                {
                    stopwatch.Start();
                    var testStepResult = await testStep.Run().ConfigureAwait(false);
                    stopwatch.Stop();

                    testStepResults.Add(testStepResult);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    testStepResults.Add(new TestStepResult(testStep, ex, stopwatch.Elapsed));
                }
            }

            return new TestResult(testStepResults.ToArray());
        }
    }

    public interface IThenTestStep : ITestStep
    {
    }

    public interface IWhenTestStep : ITestStep
    {
    }

    public interface IGivenTestStep : ITestStep
    {
    }
}