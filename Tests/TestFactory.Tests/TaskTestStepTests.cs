using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace TestFactory.Tests
{
    public class TaskTestStepTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public TaskTestStepTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ShouldRunTaskTestStep()
        {
            // Arrange
            var taskFinished = false;
            var oneSecond = TimeSpan.FromSeconds(1);

            Func<Task> createTask = () => Task.Run(async () =>
            {
                await Task.Delay(oneSecond);
                this.testOutputHelper.WriteLine($"TaskTestStep finished on Thread {Thread.CurrentThread.ManagedThreadId}");
                taskFinished = true;
            });
            var taskTestStep = new TaskTestStep(createTask);

            // Act
            var testStepResult = await taskTestStep.Run();

            // Assert
            this.testOutputHelper.WriteLine(testStepResult.ToString());

            testStepResult.Should().NotBeNull();
            testStepResult.IsSuccessful.Should().BeTrue();
            testStepResult.Duration.Should().BeCloseTo(oneSecond, precision: TimeSpan.FromMilliseconds(150));
            taskFinished.Should().BeTrue();
        }
    }
}