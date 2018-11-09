using System;
using System.Threading.Tasks;

namespace TestFactory.Tests.Testdata
{
    public class UnhandledExceptionTestStep : ITestStep
    {
        public Task<ITestStepResult> Run()
        {
            throw new InvalidOperationException("An unhandled exception occurred");
        }
    }
}