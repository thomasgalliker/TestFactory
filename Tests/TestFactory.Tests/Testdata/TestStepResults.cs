using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory.Tests.Testdata
{
    internal static class TestStepResults
    {
        internal static TestStepResult GetTestStepResult_Successful(ITestStep testStep)
        {
            return new TestStepResult(
                testStep: testStep,
                duration: new TimeSpan(0, 0, 0, 10, 999),
                isSuccessful: true, 
                result: "some random result payload",
                exception: null);
        }

        internal static TestStepResult GetTestStepResult_Failed(ITestStep testStep)
        {
            return new TestStepResult(
                testStep: testStep,
                duration: new TimeSpan(0, 0, 1, 30, 123),
                isSuccessful: false, 
                result: "some random result payload",
                exception: new InvalidOperationException("Something went wrong!"));
        }

        internal static IEnumerable<TestStepResult> GetTestStepResults(ITestStep testStep, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var testStepResultSuccessful = GetTestStepResult_Successful(testStep);
                yield return testStepResultSuccessful;

                var testStepResultFailed = GetTestStepResult_Failed(testStep);
                yield return testStepResultFailed;
            }
        }
    }
}
