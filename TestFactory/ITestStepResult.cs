using System;

namespace TestFactory
{
    public interface ITestStepResult : IFormattable
    {
        ITestStep TestStep { get; }

        object Result { get; }

        bool IsSuccessful { get; }

        Exception Exception { get; }

        TimeSpan? Duration { get; }
    }
}