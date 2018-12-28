using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace TestFactory.Tests.TestSteps
{
    public class TestStepX : TestStepBase
    {
        private readonly ITestOutputHelper testOutputHelper;
        private readonly int stepNumber;

        public TestStepX(ITestOutputHelper testOutputHelper, int stepNumber)
        {
            this.testOutputHelper = testOutputHelper;
            this.stepNumber = stepNumber;
        }

        public override async Task<ITestStepResult> Run()
        {
            await Task.Delay(1000);
            this.testOutputHelper.WriteLine($"Step{this.stepNumber} finished on Thread {Thread.CurrentThread.ManagedThreadId}");

            return new TestStepResult(this, null, isSuccessful: true);
        }
    }
}