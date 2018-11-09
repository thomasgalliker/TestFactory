using System;
using System.Collections.Generic;

namespace TestFactory
{
    public interface ITestResult : IFormattable
    {
        IEnumerable<ITestStepResult> TestStepResults { get; }

        bool IsSuccessful { get; }

        AggregateException Exception { get; }

        TimeSpan Duration { get; }
    }
}