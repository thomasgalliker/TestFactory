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
        private static readonly string Intent = FormattingHelper.Indent(8);

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

        /// <summary>
        ///     Returns the string representation of <see cref="TestResult" />.
        /// </summary>
        /// <returns>The string representation of <see cref="TestResult" /></returns>
        public override string ToString()
        {
            return this.ToString(Formats.Full);
        }

        /// <summary>
        ///     Returns the string representation of <see cref="TestResult" />.
        ///     Use <see cref="Formats" /> to select a specific formatting.
        /// </summary>
        /// <param name="format">Formatting of the string representation.</param>
        /// <returns>The string representation of <see cref="TestResult" /></returns>
        public string ToString(string format)
        {
            return this.ToString(format, null);
        }

        /// <summary>
        ///     Returns the string representation of <see cref="TestResult" />.
        ///     Use <see cref="Formats" /> to select a specific formatting.
        /// </summary>
        /// <param name="format">Formatting of the string representation.</param>
        /// <param name="provider">(Not implemented)</param>
        /// <returns>The string representation of <see cref="TestResult" /></returns>
        public string ToString(string format, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = Formats.Full;
            }

            var stringBuilder = new StringBuilder();

            if (format == Formats.Full || format == Formats.SummaryOnly)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Test Result Summary:");
                stringBuilder.AppendLine("--------------------");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"Overall success: {this.IsSuccessful}");
                stringBuilder.AppendLine($"Overall duration: {this.Duration}");
                stringBuilder.AppendLine($"Number of test steps: {this.TestStepResults.Length} (successful: {this.TestStepResults.Count(r => r.IsSuccessful)} / failed: {this.TestStepResults.Count(r => !r.IsSuccessful)})");
                stringBuilder.AppendLine();

                if (format == Formats.SummaryOnly)
                {
                    return stringBuilder.ToString();
                }
            }

            foreach (var testStepResult in this.TestStepResults)
            {
                stringBuilder.AppendLine(testStepResult.ToString());
            }

            return stringBuilder.ToString();
        }
    }
}