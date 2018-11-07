using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        public ITestResult Run()
        {
            var testStepResults = new List<ITestStepResult>();
            foreach (var testStep in this.testSteps)
            {
                try
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var testStepResult = testStep.Run();
                    stopwatch.Stop();

                    var result = testStepResult as TestStepResult;
                    if (result != null)
                    {
                        result.Duration = stopwatch.Elapsed;
                    }

                    testStepResults.Add(testStepResult);
                }
                catch (Exception ex)
                {
                    testStepResults.Add(new TestStepResult(testStep, ex));
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