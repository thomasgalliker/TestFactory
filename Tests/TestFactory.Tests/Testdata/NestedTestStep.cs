using System;
using System.Threading.Tasks;

namespace TestFactory.Tests.Testdata
{
    public class NestedTestStep : ITestStep
    {
        public async Task<ITestStepResult> Run()
        {
            var nestedSystemTestBuilder = new SystemTestBuilder();
            nestedSystemTestBuilder.AddTestStep(() => { });
            nestedSystemTestBuilder.AddTestStep(() => { });
            nestedSystemTestBuilder.AddTestStep(() => { throw new NotSupportedException(); });

            var nestedTestResult = await nestedSystemTestBuilder.Run();

            return new TestStepResult(this, nestedTestResult);
        }
    }
}