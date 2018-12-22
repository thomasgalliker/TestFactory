
using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldCallActionOnRun()
        {
            // Arrange
            var actionCalled = false;
            Action action = () => { actionCalled = true; };
            ITestStep actionTestStep = new ActionTestStep(action);

            // Act
            ITestStepResult testStepResult = await actionTestStep.Run();

            // Assert
            this.testOutputHelper.WriteLine(testStepResult.ToString());
            testStepResult.Should().NotBeNull();
            testStepResult.IsSuccessful.Should().BeTrue();

            actionCalled.Should().BeTrue();
        }
    }
}