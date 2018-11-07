using System;
using FluentAssertions;
using TestFactory.Tests.Testdata;
using Xunit;

namespace TestFactory.Tests
{
    public class TestResultTests
    {
        [Fact]
        public void ShouldThrowException_MustHaveAtLeastOneResult()
        {
            // Act
            Action action = () => new TestResult();

            // Assert
            action.Should().Throw<ArgumentException>().Which.Message.Should().Contain("Must have at least 1 test step result");
        }

        [Fact]
        public void ShouldToString_FormatsFull()
        {
            // Arrange
            ITestStep testStep = new EmptyActionTestStep();
            ITestStepResult testStepResult = new TestStepResult(testStep);
            ITestResult testResult = new TestResult(testStepResult);

            // Act
            var testResultString = testResult.ToString(Formats.Full, null);

            // Assert
            testResultString.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldToString_FormatsCompact()
        {
            // Arrange
            ITestStep testStep = new EmptyActionTestStep();
            ITestStepResult testStepResult = new TestStepResult(testStep);
            ITestResult testResult = new TestResult(testStepResult);

            // Act
            var testResultString = testResult.ToString(Formats.Compact, null);

            // Assert
            testResultString.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldToString_FormatsSummaryOnly()
        {
            // Arrange
            ITestStep testStep = new EmptyActionTestStep();
            ITestStepResult testStepResult = new TestStepResult(testStep);
            ITestResult testResult = new TestResult(testStepResult);

            // Act
            var testResultString = testResult.ToString(Formats.SummaryOnly, null);

            // Assert
            testResultString.Should().NotBeNullOrEmpty();
        }
    }
}