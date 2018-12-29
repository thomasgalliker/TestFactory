using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace TestFactory.Tests.TestSteps
{
    public class ParallelTestStepTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public ParallelTestStepTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ShouldRunParallelTestStep_WithEmptyTaskList()
        {
            // Arrange
            IEnumerable<ITestStep> parallelTestSteps = new List<ITestStep>();

            var parallelTestStep = new ParallelTestStep(parallelTestSteps);

            // Act
            var testStepResult = await parallelTestStep.Run();

            // Assert
            this.testOutputHelper.WriteLine(testStepResult.ToString());

            testStepResult.Should().NotBeNull();
            testStepResult.IsSuccessful.Should().BeTrue();
            testStepResult.Duration.Should().BeCloseTo(TimeSpan.FromMilliseconds(150));
        }

        [Fact]
        public async Task ShouldRunParallelTestStep_WithTaskList()
        {
            // Arrange
            var oneSecond = TimeSpan.FromSeconds(1);
            IEnumerable<ITestStep> parallelTestSteps = new List<ITestStep>
            {
                new TestStepX(this.testOutputHelper, 1),
                new TestStepX(this.testOutputHelper, 2),
                new TestStepX(this.testOutputHelper, 3),
                new TestStepX(this.testOutputHelper, 4),
                new TestStepX(this.testOutputHelper, 5),
            };

            var parallelTestStep = new ParallelTestStep(parallelTestSteps);

            // Act
            var testStepResult = await parallelTestStep.Run();

            // Assert
            this.testOutputHelper.WriteLine(testStepResult.ToString());

            testStepResult.Should().NotBeNull();
            testStepResult.IsSuccessful.Should().BeTrue();
            testStepResult.Duration.Should().BeCloseTo(oneSecond, precision: TimeSpan.FromMilliseconds(150));
        }
    }
}