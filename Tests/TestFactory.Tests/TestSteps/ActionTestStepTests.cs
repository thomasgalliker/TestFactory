
using System;
using FluentAssertions;
using TestFactory.TestSteps;
using Xunit;
using Xunit.Abstractions;

namespace TestFactory.Tests.TestSteps
{
    public class ActionTestStepTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ActionTestStepTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ShouldCallActionOnRun()
        {
            // Arrange
            var actionCalled = false;
            Action action = () => { actionCalled = true; };
            ITestStep actionTestStep = new ActionTestStep(action);

            // Act
            ITestStepResult testStepResult = actionTestStep.Run();

            // Assert
            this.testOutputHelper.WriteLine(testStepResult.ToString());
            testStepResult.Should().NotBeNull();
            //testStepResult.Duration.Should().BeGreaterOrEqualTo(timeSpan);

            actionCalled.Should().BeTrue();
        }
    }
}