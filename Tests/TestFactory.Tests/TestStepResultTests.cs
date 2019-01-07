using FluentAssertions;
using TestFactory.Tests.Testdata;
using TestFactory.Tests.TestSteps;
using Xunit;
using Xunit.Abstractions;

namespace TestFactory.Tests
{
    public class TestStepResultTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public TestStepResultTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ShouldToString()
        {
            // Arrange
            ITestStep testStep = new EmptyActionTestStep();
            ITestStepResult testStepResult = TestStepResults.GetTestStepResult_Successful(testStep);

            // Act
            var testStepResultString = testStepResult.ToString(Formats.Full, null);

            // Assert
            this.testOutputHelper.WriteLine(testStepResultString);
            testStepResultString.Should().NotBeNullOrEmpty();
            testStepResultString.Should().Contain("-> EmptyActionTestStep:");
        }

        [Fact]
        public void ShouldToString_GenericClass()
        {
            // Arrange
            ITestStep testStep = new GenericTestStep<MyClass>();
            ITestStepResult testStepResult = TestStepResults.GetTestStepResult_Successful(testStep);

            // Act
            var testStepResultString = testStepResult.ToString(Formats.Full, null);

            // Assert
            this.testOutputHelper.WriteLine(testStepResultString);
            testStepResultString.Should().NotBeNullOrEmpty();
            testStepResultString.Should().Contain("-> GenericTestStep<MyClass>:");
        }
    }
}