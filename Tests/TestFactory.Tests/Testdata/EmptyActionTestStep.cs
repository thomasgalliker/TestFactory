namespace TestFactory.Tests.Testdata
{
    public class EmptyActionTestStep : ActionTestStep
    {
        public EmptyActionTestStep() : base(() => { })
        {
        }
    }
}
