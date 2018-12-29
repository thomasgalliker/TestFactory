using System;
using System.Threading.Tasks;

namespace TestFactory.TestSteps
{
    public class CompletedTestStep : TestStepBase
    {
        public override Task<ITestStepResult> Run()
        {
            return Task.FromResult<ITestStepResult>(new TestStepResult(this, duration: TimeSpan.Zero, isSuccessful: true));
        }
    }
}
