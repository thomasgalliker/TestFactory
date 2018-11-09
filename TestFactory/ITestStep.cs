using System.Threading.Tasks;

namespace TestFactory
{
    public interface ITestStep
    {
        Task<ITestStepResult> Run();
    }
}