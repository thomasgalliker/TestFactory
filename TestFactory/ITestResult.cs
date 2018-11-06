using System;

namespace TestFactory
{
    public interface ITestResult
    {
        ITestStepResult[] TestStepResults { get; }

        bool IsSuccessful { get; }

        AggregateException Exception { get; }

        TimeSpan Duration { get; }
    }
}