using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TestFactory.Extensions;

namespace TestFactory
{
    [DebuggerDisplay("TestResult: IsSuccessful={this.IsSuccessful}, Duration={this.Duration}")]
    public class TestResult : ITestResult
    {
        public TestResult(params ITestStepResult[] testStepResults)
        {
            if (testStepResults == null)
            {
                throw new ArgumentNullException(nameof(testStepResults));
            }

            if (!testStepResults.Any())
            {
                throw new ArgumentException("Must have at least 1 test step result.", nameof(testStepResults));
            }

            this.TestStepResults = testStepResults;
            this.IsSuccessful = this.TestStepResults.All(r => r.IsSuccessful);
            this.Exception = new AggregateException(this.TestStepResults.Where(r => r.Exception != null).Select(r => r.Exception));
            this.Duration = testStepResults.Sum(r => r.Duration.GetValueOrDefault());
        }

        public ITestStepResult[] TestStepResults { get; }

        public bool IsSuccessful { get; }

        public AggregateException Exception { get; }

        public TimeSpan Duration { get; }

        private static string Indent(int count)
        {
            return "".PadLeft(count);
        }

        private static readonly string Intent = Indent(8);

        public class Formats
        {
            public const string Full = "F";
            public const string SummaryOnly = "S";
            public const string Compact = "C";
        }

        public override string ToString()
        {
            return this.ToString(Formats.Full);
        }

        public string ToString(string format = null, IFormatProvider provider = null)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = Formats.Full;
            }

            var stringBulider = new StringBuilder();

            if (format == Formats.Full || format == Formats.SummaryOnly)
            {
                stringBulider.AppendLine();
                stringBulider.AppendLine("Test Result Summary:");
                stringBulider.AppendLine("--------------------");
                stringBulider.AppendLine();
                stringBulider.AppendLine($"Overall success: {this.IsSuccessful}");
                stringBulider.AppendLine($"Overall duration: {this.Duration}");
                stringBulider.AppendLine($"Number of test steps: {this.TestStepResults.Length} (successful: {this.TestStepResults.Count(r => r.IsSuccessful)} / failed: {this.TestStepResults.Count(r => !r.IsSuccessful)})");
                stringBulider.AppendLine();

                if (format == Formats.SummaryOnly)
                {
                    return stringBulider.ToString();
                }
            }

            foreach (var testStepResult in this.TestStepResults)
            {
                stringBulider.AppendLine($"-> {testStepResult.TestStep.GetType().GetFormattedName()}:\t\t\tIsSuccessful = {testStepResult.IsSuccessful},\t\t\tDuration = {testStepResult.Duration}");
                if (testStepResult.IsSuccessful == false)
                {
                    stringBulider.Append($"{Intent}{testStepResult.Exception.ToString().Replace("\n", "\n" + Intent)}");
                    stringBulider.AppendLine();
                    stringBulider.AppendLine();
                }

                var nestedTestResult = testStepResult.Result as TestResult;
                if (nestedTestResult != null)
                {
                    var nestedString = nestedTestResult.ToString(Formats.Compact);
                    foreach (var line in nestedString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
                    {
                        stringBulider.AppendLine($"{Intent}{line}");
                    }
                    stringBulider.AppendLine();
                }
            }

            return stringBulider.ToString();
        }
    }
}