using System;
using System.Linq;
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
        public void ShouldPartiallyFailIfLastStepFails()
        {
            // Arrange
            var systemTestBuilder = new SystemTestBuilder()
                    .AddTestStep(() => { })
                    .AddTestStep(() => { throw new Exception("Something failed"); })
                ;

            // Act
            var testResult = systemTestBuilder.Run();

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
        public void ShouldPartiallyFailIfFirstStepFails()
        {
            // Arrange
            var systemTestBuilder = new SystemTestBuilder()
                    .AddTestStep(() => { throw new Exception("Something failed"); })
                    .AddTestStep(() => { })
                    .AddTestStep(() => { })
                ;

            // Act
            var testResult = systemTestBuilder.Run();

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
        public void ShouldCatchUnhandledException()
        {
            // Arrange
            var systemTestBuilder = new SystemTestBuilder()
                    .AddTestStep(new UnhandledExceptionTestStep())
                    ;

            // Act
            var testResult = systemTestBuilder.Run();

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
        public void ShouldRunNestedTests()
        {
            // Arrange
            var systemTestBuilder = new SystemTestBuilder()
                    .AddTestStep(new NestedTestStep())
                    .AddTestStep(new NestedTestStep())
                ;

            // Act
            var testResult = systemTestBuilder.Run();

            // Assert
            testResult.IsSuccessful.Should().BeFalse();
            testResult.TestStepResults.Should().HaveCount(2);
            testResult.TestStepResults.ElementAt(0).Result.Should().NotBeNull();
            testResult.TestStepResults.ElementAt(1).Result.Should().NotBeNull();

            this.testOutputHelper.WriteLine(testResult.ToString());
        }
    }
}