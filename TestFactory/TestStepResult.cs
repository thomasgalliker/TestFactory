using System;
using System.Diagnostics;
using System.Text;
using TestFactory.Extensions;

namespace TestFactory
{
    [DebuggerDisplay("TestStepResult: IsSuccessful={this.IsSuccessful}, Duration={this.Duration}")]
    public class TestStepResult : ITestStepResult
    {
        private static readonly string Intent = FormattingHelper.Indent(8);

        private readonly bool isSuccessful;

        public TestStepResult(ITestStep testStep, bool isSuccessful)
        {
            this.TestStep = testStep;
            this.isSuccessful = isSuccessful;
        }

        public TestStepResult(ITestStep testStep, object result = null)
        {
            this.TestStep = testStep;
            this.Result = result;
        }

        public TestStepResult(ITestStep testStep, ITestResult nestedTestResult)
        {
            this.TestStep = testStep;
            this.Result = nestedTestResult;
            this.isSuccessful = nestedTestResult.IsSuccessful;
            this.Exception = nestedTestResult.Exception;
            this.Duration = nestedTestResult.Duration;
        }

        public TestStepResult(ITestStep testStep, Exception exception)
        {
            this.TestStep = testStep;
            this.Exception = exception;
        }

        public object Result { get; }

        public Exception Exception { get; }

        public ITestStep TestStep { get; }

        public bool IsSuccessful
        {
            get { return this.isSuccessful || this.Exception == null; }
        }

        public TimeSpan? Duration { get; internal set; }

        public override string ToString()
        {
            return this.ToString(null, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"-> {this.TestStep.GetType().GetFormattedName()}:\t\t\tIsSuccessful = {this.IsSuccessful},\t\t\tDuration = {this.Duration}");
            if (this.IsSuccessful == false)
            {
                stringBuilder.Append($"{Intent}{this.Exception.ToString().Replace("\n", "\n" + Intent)}");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine();
            }

            var nestedTestResult = this.Result as TestResult;
            if (nestedTestResult != null)
            {
                var nestedString = nestedTestResult.ToString(Formats.Compact);
                foreach (var line in nestedString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
                {
                    stringBuilder.AppendLine($"{Intent}{line}");
                }
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}