using System;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldWaitOnWaitStepRun_FromTimeSpan()
        {
            // Arrange
            var timeSpan = new TimeSpan(0, 0, 1);
            ITestStep waitStep = new WaitStep(timeSpan);

            // Act
            var testStepResult = await waitStep.Run();

            // Assert
            this.testOutputHelper.WriteLine(testStepResult.ToString());
            testStepResult.Should().NotBeNull();
#if DEBUG
            testStepResult.Duration.Should().BeGreaterThan(timeSpan);
#else
            testStepResult.Duration.Should().BeGreaterThan(TimeSpan.Zero);
#endif
        }

        [Fact]
        public async Task ShouldWaitOnWaitStepRun_FromMilliseconds()
        {
            // Arrange
            const int milliseconds = 999;
            ITestStep waitStep = new WaitStep(milliseconds);

            // Act
            ITestStepResult testStepResult = await waitStep.Run();

            // Assert
            this.testOutputHelper.WriteLine(testStepResult.ToString());
            testStepResult.Should().NotBeNull();

#if DEBUG
            testStepResult.Duration.Should().BeGreaterThan(TimeSpan.FromMilliseconds(milliseconds));
#else
            testStepResult.Duration.Should().BeGreaterThan(TimeSpan.Zero);
#endif
        }
    }
}
