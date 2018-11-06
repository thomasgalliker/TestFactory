using System;

namespace TestFactory.TestSteps
{
    public class ActionTestStep : ITestStep
    {
        private readonly Action action;

        public ActionTestStep(Action action)
        {
            this.action = action;
        }

        public ITestStepResult Run()
        {
            this.action();

            return new TestStepResult(this);
        }
    }
}