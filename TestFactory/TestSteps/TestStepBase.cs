using System.Diagnostics;
using System.Threading.Tasks;
using TestFactory.Internal;

namespace TestFactory
{
    [DebuggerDisplay("TestStep: {this.Name}")]
    public abstract class TestStepBase : ITestStep
    {
        protected TestStepBase(string name = null)
        {
            this.Name = name ?? this.GetType().GetFormattedName();
        }

        public string Name { get; }

        public abstract Task<ITestStepResult> Run();
    }
}