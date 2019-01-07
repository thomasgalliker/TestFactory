using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace TestFactory.Tests.TestSteps
{
    public class CompletedTestStepTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public CompletedTestStepTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ShouldRunCompletedTestStep()
        {
            // Arrange
            ITestStep completedTestStep = new CompletedTestStep();

            // Act
            var testStepResult = await completedTestStep.Run();

            // Assert
            this.testOutputHelper.WriteLine(testStepResult.ToString());

            testStepResult.Should().NotBeNull();
            testStepResult.IsSuccessful.Should().BeTrue();
            testStepResult.Duration.Should().Be(TimeSpan.Zero);
        }
    }
}