using System.Threading.Tasks;

namespace TestFactory
{
    public interface ITestStep
    {
        string Name { get; }

        Task<ITestStepResult> Run();
    }
}