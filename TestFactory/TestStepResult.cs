using System;
using System.Diagnostics;

namespace TestFactory
{
    [DebuggerDisplay("TestStepResult: IsSuccessful={this.IsSuccessful}, Duration={this.Duration}")]
    public class TestStepResult : ITestStepResult
    {
        private readonly bool isSuccessful;

        public TestStepResult(ITestStep testStep, bool isSuccessful)
        {
            this.TestStep = testStep;
            this.isSuccessful = isSuccessful;
        }

        public TestStepResult(ITestStep testStep, object result = null)
        {
            this.TestStep = testStep;
            this.Result = result;
        }

        public TestStepResult(ITestStep testStep, ITestResult nestedTestResult)
        {
            this.TestStep = testStep;
            this.Result = nestedTestResult;
            this.isSuccessful = nestedTestResult.IsSuccessful;
            this.Exception = nestedTestResult.Exception;
            this.Duration = nestedTestResult.Duration;
        }

        public TestStepResult(ITestStep testStep, Exception exception)
        {
            this.TestStep = testStep;
            this.Exception = exception;
        }

        public object Result { get; }

        public Exception Exception { get; }

        public ITestStep TestStep { get; }

        public bool IsSuccessful
        {
            get { return this.isSuccessful || this.Exception == null; }
        }

        public TimeSpan? Duration { get; internal set; }
    }
}