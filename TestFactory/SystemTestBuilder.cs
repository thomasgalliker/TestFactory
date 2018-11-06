using System;
using System.Collections.Generic;
using System.Diagnostics;
using TestFactory.TestSteps;

namespace TestFactory
{
    public class SystemTestBuilder
    {
        private readonly List<ITestStep> testSteps;

        public SystemTestBuilder()
        {
            this.testSteps = new List<ITestStep>();
        }

        public SystemTestBuilder AddTestStep(Action actionTestStep)
        {
            this.testSteps.Add(new ActionTestStep(actionTestStep));

            return this;
        }

        public SystemTestBuilder AddTestStep(ITestStep testStep)
        {
            this.testSteps.Add(testStep);

            return this;
        }

        public SystemTestBuilder Given(IGivenTestStep givenTestStep)
        {
            return this.AddTestStep(givenTestStep);
        }

        public SystemTestBuilder When(IWhenTestStep whenTestStep)
        {
            return this.AddTestStep(whenTestStep);
        }

        public SystemTestBuilder Then(IThenTestStep thenTestStep)
        {
            return this.AddTestStep(thenTestStep);
        }

        public SystemTestBuilder Then(Action thenTestStep)
        {
            return this.AddTestStep(thenTestStep);
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