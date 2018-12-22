using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using TestFactory.Tests.Testdata;
using Xunit;
using Xunit.Abstractions;

namespace TestFactory.Tests
{
    public class SystemTestBuilderTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public SystemTestBuilderTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ShouldPartiallyFailIfLastStepFails()
        {
            // Arrange
            var systemTestBuilder = new SystemTestBuilder()
                    .AddTestStep(() => { })
                    .AddTestStep(() => { throw new Exception("Something failed"); })
                ;

            // Act
            var testResult = await systemTestBuilder.Run();

            // Assert
            testResult.IsSuccessful.Should().BeFalse();
            testResult.TestStepResults.Should().HaveCount(2);
            testResult.TestStepResults.ElementAt(0).IsSuccessful.Should().BeTrue();
            testResult.TestStepResults.ElementAt(1).IsSuccessful.Should().BeFalse();
            testResult.Exception.InnerExceptions.Should().HaveCount(1);
            testResult.Exception.InnerExceptions[0].Message.Should().Contain("Something failed");

            this.testOutputHelper.WriteLine(testResult.ToString());
        }

        [Fact]
        public async Task ShouldPartiallyFailIfFirstStepFails()
        {
            // Arrange
            var systemTestBuilder = new SystemTestBuilder()
                    .AddTestStep(() => { throw new Exception("Something failed"); })
                    .AddTestStep(() => { })
                    .AddTestStep(() => { })
                ;

            // Act
            var testResult = await systemTestBuilder.Run();

            // Assert
            testResult.IsSuccessful.Should().BeFalse();
            testResult.TestStepResults.Should().HaveCount(3);
            testResult.TestStepResults.ElementAt(0).IsSuccessful.Should().BeFalse();
            testResult.TestStepResults.ElementAt(1).IsSuccessful.Should().BeTrue();
            testResult.Exception.InnerExceptions.Should().HaveCount(1);
            testResult.Exception.InnerExceptions[0].Message.Should().Contain("Something failed");

            this.testOutputHelper.WriteLine(testResult.ToString());
        }

        [Fact]
        public async Task ShouldCatchUnhandledException()
        {
            // Arrange
            var systemTestBuilder = new SystemTestBuilder()
                    .AddTestStep(new UnhandledExceptionTestStep())
                    ;

            // Act
            var testResult = await systemTestBuilder.Run();

            // Assert
            testResult.IsSuccessful.Should().BeFalse();
            testResult.TestStepResults.Should().HaveCount(1);
            testResult.TestStepResults.ElementAt(0).IsSuccessful.Should().BeFalse();
            testResult.Exception.InnerExceptions.Should().HaveCount(1);
            testResult.Exception.InnerExceptions[0].Message.Should().Contain("An unhandled exception occurred");
            testResult.Exception.InnerExceptions[0].Should().BeOfType<InvalidOperationException>();

            this.testOutputHelper.WriteLine(testResult.ToString());
        }

        [Fact]
        public async Task ShouldRunNestedTests()
        {
            // Arrange
            var systemTestBuilder = new SystemTestBuilder()
                    .AddTestStep(new NestedTestStep())
                    .AddTestStep(new NestedTestStep())
                ;

            // Act
            var testResult = await systemTestBuilder.Run();

            // Assert
            testResult.IsSuccessful.Should().BeFalse();
            testResult.TestStepResults.Should().HaveCount(2);
            testResult.TestStepResults.ElementAt(0).Result.Should().NotBeNull();
            testResult.TestStepResults.ElementAt(1).Result.Should().NotBeNull();

            this.testOutputHelper.WriteLine(testResult.ToString());
        }

        [Fact]
        public async Task ShouldRunTestStepsInParallel()
        {
            // Arrange
            var oneSecond = TimeSpan.FromSeconds(1);
            var systemTestBuilder = new SystemTestBuilder()
                    .AddTestStepAsync(async () => { await Task.Delay(1000); this.testOutputHelper.WriteLine($"Step1 (sync) finished on Thread {Thread.CurrentThread.ManagedThreadId}"); })
                    .AddTestStepAsync(async () => { await Task.Delay(1000); this.testOutputHelper.WriteLine($"Step2 (sync) finished on Thread {Thread.CurrentThread.ManagedThreadId}"); })
                    .AddParallelTestStep(
                        async () => { await Task.Delay(1000); this.testOutputHelper.WriteLine($"Step3 (async) finished on Thread {Thread.CurrentThread.ManagedThreadId}"); },
                        async () => { await Task.Delay(1000); this.testOutputHelper.WriteLine($"Step4 (async) finished on Thread {Thread.CurrentThread.ManagedThreadId}"); },
                        async () => { await Task.Delay(1000); this.testOutputHelper.WriteLine($"Step5 (async) finished on Thread {Thread.CurrentThread.ManagedThreadId}"); },
                        async () => { await Task.Delay(1000); this.testOutputHelper.WriteLine($"Step6 (async) finished on Thread {Thread.CurrentThread.ManagedThreadId}"); },
                        async () => { await Task.Delay(1000); this.testOutputHelper.WriteLine($"Step7 (async) finished on Thread {Thread.CurrentThread.ManagedThreadId}"); })
                ;

            // Act
            var testResult = await systemTestBuilder.Run();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
            testResult.TestStepResults.Should().HaveCount(3);
            testResult.TestStepResults.ElementAt(0).Duration.Should().BeCloseTo(oneSecond, precision: TimeSpan.FromMilliseconds(150));
            testResult.TestStepResults.ElementAt(1).Duration.Should().BeCloseTo(oneSecond, precision: TimeSpan.FromMilliseconds(150));
            testResult.TestStepResults.ElementAt(2).Duration.Should().BeCloseTo(oneSecond, precision: TimeSpan.FromMilliseconds(200));

            this.testOutputHelper.WriteLine(testResult.ToString());
        }
    }
}