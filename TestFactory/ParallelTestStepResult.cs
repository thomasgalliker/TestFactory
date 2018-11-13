using System;
using System.Diagnostics;
using System.Text;

namespace TestFactory
{
    [DebuggerDisplay("TestStepResult: IsSuccessful={this.IsSuccessful}, Duration={this.Duration}")]
    public class ParallelTestStepResult : TestStepResult
    {
        private static readonly string Intent = "".PadLeft(8);
        private readonly ITestStepResult[] testStepResults;

        public ParallelTestStepResult(ParallelTestStep parallelTestStep, TimeSpan? duration, bool isSuccessful, ITestStepResult[] testStepResults, Exception exception)
            : base(parallelTestStep, duration, isSuccessful, exception)
        {
            this.testStepResults = testStepResults;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(base.ToString());
            foreach (var testStepResult in this.testStepResults)
            {
                stringBuilder.AppendLine(Intent + "|" + testStepResult);
            }

            return stringBuilder.ToString();
        }
    }
}