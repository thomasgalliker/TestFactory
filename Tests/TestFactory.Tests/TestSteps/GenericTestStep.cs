using System;
using System.Threading.Tasks;

namespace TestFactory.Tests.TestSteps
{
    public class GenericTestStep<T> : TestStepBase
    {
        public override Task<ITestStepResult> Run()
        {
            return Task.FromResult<ITestStepResult>(new TestStepResult(this, TimeSpan.FromMinutes(2), isSuccessful: true));
        }
    }
}