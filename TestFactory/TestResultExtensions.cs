using System;

namespace TestFactory
{
    public static class TestResultExtensions
    {
        public static SystemTestBuilder ToFile(this ITestResult testResult, string path)
        {
            var testResultString = testResult.ToString();
            throw new NotImplementedException();
        }
    }
}