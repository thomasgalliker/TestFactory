using System;
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
            testResult.TestStepResults[0].IsSuccessful.Should().BeTrue();
            testResult.TestStepResults[1].IsSuccessful.Should().BeFalse();
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
            testResult.TestStepResults[0].IsSuccessful.Should().BeFalse();
            testResult.TestStepResults[1].IsSuccessful.Should().BeTrue();
            testResult.Exception.InnerExceptions.Should().HaveCount(1);
            testResult.Exception.InnerExceptions[0].Message.Should().Contain("Something failed");

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
            testResult.TestStepResults[0].Result.Should().NotBeNull();
            testResult.TestStepResults[1].Result.Should().NotBeNull();

            this.testOutputHelper.WriteLine(testResult.ToString());
        }
    }
}