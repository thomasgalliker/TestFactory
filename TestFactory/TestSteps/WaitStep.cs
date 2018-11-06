
namespace TestFactory.TestSteps
{
    public class WaitStep : ITestStep
    {
        private readonly int time;

        public WaitStep(int time)
        {
            this.time = time;
        }

        public ITestStepResult Run()
        {
            //Thread.Sleep(this.time);
            return new TestStepResult(this);
        }
    }
}