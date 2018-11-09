using System;

namespace TestFactory.Tests.Testdata
{
    public class UnhandledExceptionTestStep : ITestStep
    {
        public ITestStepResult Run()
        {
            throw new InvalidOperationException("An unhandled exception occurred");
        }
    }
}