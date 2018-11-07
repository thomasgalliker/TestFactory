using System;

namespace TestFactory.Tests.Testdata
{
    public class NestedTestStep : ITestStep
    {
        public ITestStepResult Run()
        {
            var nestedSystemTestBuilder = new SystemTestBuilder();
            nestedSystemTestBuilder.AddTestStep(() => { });
            nestedSystemTestBuilder.AddTestStep(() => { });
            nestedSystemTestBuilder.AddTestStep(() => { throw new NotSupportedException(); });

            var nestedTestResult = nestedSystemTestBuilder.Run();

            return new TestStepResult(this, nestedTestResult);
        }
    }
}