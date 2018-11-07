using System;
using FluentAssertions;
using TestFactory.TestSteps;
using Xunit;
using Xunit.Abstractions;

namespace TestFactory.Tests.TestSteps
{
    public class WaitStepTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public WaitStepTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ShouldWaitOnWaitStepRun()
        {
            // Arrange
            var timeSpan = new TimeSpan(0, 0, 1);
            ITestStep waitStep = new WaitStep(timeSpan);

            // Act
            ITestStepResult testStepResult = waitStep.Run();

            // Assert
            this.testOutputHelper.WriteLine(testStepResult.ToString());
            testStepResult.Should().NotBeNull();
            //testStepResult.Duration.Should().BeGreaterOrEqualTo(timeSpan);
        }
    }
}