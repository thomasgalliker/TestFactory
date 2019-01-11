using System;
using FluentAssertions;
using TestFactory.Tests.Testdata;
using Xunit;
using Xunit.Abstractions;

namespace TestFactory.Tests
{
    public class TestResultTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public TestResultTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ShouldCreateTestResult_NoTestStepResults()
        {
            // Act
            ITestResult testResult = new TestResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
            testResult.Exception.Should().BeNull();
            testResult.Duration.Should().Be(TimeSpan.Zero);
        }

        [Fact]
        public void ShouldCreateTestResult_MultipleTestStepResults_Success()
        {
            // Arrange
            ITestStep testStep = new EmptyActionTestStep();
            ITestStepResult testStepResult1 = TestStepResults.GetTestStepResult_Successful(testStep);
            ITestStepResult testStepResult2 = TestStepResults.GetTestStepResult_Successful(testStep);

            // Act
            ITestResult testResult = new TestResult(testStepResult1, testStepResult2);

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
            testResult.Exception.Should().BeNull();
            testResult.Duration.Should().Be(new TimeSpan(0, 0, 0, 21, 998));
        }

        [Fact]
        public void ShouldCreateTestResult_MultipleTestStepResults_Failed()
        {
            // Arrange
            ITestStep testStep = new EmptyActionTestStep();
            ITestStepResult testStepResult1 = TestStepResults.GetTestStepResult_Successful(testStep);
            ITestStepResult testStepResult2 = TestStepResults.GetTestStepResult_Failed(testStep);
            ITestStepResult testStepResult3 = TestStepResults.GetTestStepResult_Failed(testStep);

            // Act
            ITestResult testResult = new TestResult(testStepResult1, testStepResult2, testStepResult3);

            // Assert
            testResult.IsSuccessful.Should().BeFalse();
            testResult.Exception.Should().BeOfType<AggregateException>().Which.InnerExceptions.Should().HaveCount(2);
            testResult.Duration.Should().Be(new TimeSpan(0, 0, 3, 11, 245));
        }

        [Fact]
        public void ShouldToString_FormatsFull()
        {
            // Arrange
            ITestStep testStep = new EmptyActionTestStep();
            ITestStepResult testStepResult = TestStepResults.GetTestStepResult_Successful(testStep);
            ITestResult testResult = new TestResult(testStepResult);

            // Act
            var testResultString = testResult.ToString(Formats.Full, null);

            // Assert
            this.testOutputHelper.WriteLine(testResultString);
            testResultString.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldToString_FormatsCompact()
        {
            // Arrange
            ITestStep testStep = new EmptyActionTestStep();
            ITestStepResult testStepResult = TestStepResults.GetTestStepResult_Successful(testStep);
            ITestResult testResult = new TestResult(testStepResult);

            // Act
            var testResultString = testResult.ToString(Formats.Compact, null);

            // Assert
            this.testOutputHelper.WriteLine(testResultString);
            testResultString.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldToString_FormatsSummaryOnly()
        {
            // Arrange
            ITestStep testStep = new EmptyActionTestStep();
            ITestStepResult testStepResult = TestStepResults.GetTestStepResult_Successful(testStep);
            ITestResult testResult = new TestResult(testStepResult);

            // Act
            var testResultString = testResult.ToString(Formats.SummaryOnly, null);

            // Assert
            this.testOutputHelper.WriteLine(testResultString);
            testResultString.Should().NotBeNullOrEmpty();
        }
    }
}
