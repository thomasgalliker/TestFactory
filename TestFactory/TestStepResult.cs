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

        public TestStepResult(ITestStep testStep, TimeSpan? duration, bool isSuccessful) 
            : this(testStep, duration, isSuccessful, result: null)
        {
        }

        public TestStepResult(ITestStep testStep, TimeSpan? duration, bool isSuccessful, object result) 
            : this(testStep, duration, isSuccessful, result: result, exception: null)
        {
        }

        public TestStepResult(ITestStep testStep, TimeSpan? duration, bool isSuccessful, Exception exception) 
            : this(testStep, duration, isSuccessful, result: null, exception: exception)
        {
        }

        public TestStepResult(ITestStep testStep, TimeSpan? duration, bool isSuccessful, object result, Exception exception)
        {
            this.TestStep = testStep;
            this.IsSuccessful = isSuccessful;
            this.Result = result;
            this.Exception = exception;
            this.Duration = duration;
        }

        public TestStepResult(ITestStep testStep, ITestResult nestedTestResult) 
            : this(testStep, isSuccessful: nestedTestResult.IsSuccessful, result: nestedTestResult, exception: nestedTestResult.Exception, duration: nestedTestResult.Duration)
        {
        }

        public TestStepResult(ITestStep testStep, Exception exception, TimeSpan? duration)
            : this(testStep, isSuccessful: false, result: null, exception: exception, duration: duration)
        {
        }

        public object Result { get; }

        public Exception Exception { get; }

        public ITestStep TestStep { get; }

        public bool IsSuccessful { get; }

        public TimeSpan? Duration { get; }

        public override string ToString()
        {
            return this.ToString(null, null);
        }

        public virtual string ToString(string format, IFormatProvider formatProvider)
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
