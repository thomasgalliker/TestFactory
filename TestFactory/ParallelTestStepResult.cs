using System;
using System.Diagnostics;
using System.Text;

namespace TestFactory
{
    [DebuggerDisplay("ParallelTestStepResult: IsSuccessful={this.IsSuccessful}, Duration={this.Duration}")]
    public class ParallelTestStepResult : TestStepResult
    {
        private static readonly string Intent = "".PadLeft(8);

        public ParallelTestStepResult(ParallelTestStep parallelTestStep, TimeSpan? duration, bool isSuccessful, ITestStepResult[] testStepResults, Exception exception)
            : base(parallelTestStep, duration, isSuccessful, exception)
        {
            this.TestStepResults = testStepResults;
        }

        public ITestStepResult[] TestStepResults { get; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(base.ToString());
            foreach (var testStepResult in this.TestStepResults)
            {
                stringBuilder.AppendLine(Intent + "|" + testStepResult);
            }

            return stringBuilder.ToString();
        }
    }
}