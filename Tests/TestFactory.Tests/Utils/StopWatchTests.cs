using System;
using System.Threading.Tasks;
using FluentAssertions;
using TestFactory.Utils;
using Xunit;

namespace TestFactory.Tests.Utils
{
    public class StopWatchTests
    {
        [Fact]
        public void ShouldCreateStopWatch()
        {
            // Act
            var stopwatch = new StopWatch();

            // Assert
            stopwatch.Elapsed.Should().Be(TimeSpan.Zero);
        }

        [Fact]
        public async Task ShouldCreateStopWatch_Autostart()
        {
            // Arrange
            const bool autostart = true;

            // Act
            var stopwatch = new StopWatch(autostart);
            await Task.Delay(100);

            // Assert
            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.Zero);
        }

        [Fact]
        public async Task ShouldCreateStopWatch_AutostartAndCallback_NotCalledIfNotDisposed()
        {
            // Arrange
            var isCalled = false;
            const bool autostart = true;
            Action<TimeSpan> callback = ts => { isCalled = true; };

            // Act
            var stopwatch = new StopWatch(autostart, callback);
            await Task.Delay(100);

            // Assert
            isCalled.Should().BeFalse();
            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.Zero);
        }


        [Fact]
        public async Task ShouldCreateStopWatch_AutostartAndCallback_CalledOnDispose()
        {
            // Arrange
            var isCalled = false;
            const bool autostart = true;
            Action<TimeSpan> callback = ts => { isCalled = true; };

            // Act
            var stopwatch = new StopWatch(autostart, callback);
            await Task.Delay(100);
            stopwatch.Dispose();

            // Assert
            isCalled.Should().BeTrue();
            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.Zero);
        }

        [Fact]
        public async Task ShouldStartAndStopStopwatch()
        {
            // Arrange
            var stopwatch = new StopWatch();

            // Act
            stopwatch.Start();
            await Task.Delay(100);
            stopwatch.Stop();
            await Task.Delay(100);

            // Assert
            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.FromMilliseconds(100));
            stopwatch.Elapsed.Should().BeLessThan(TimeSpan.FromMilliseconds(200));
        }

        [Fact]
        public async Task ShouldStopOnDispose()
        {
            // Arrange
            var stopwatch = new StopWatch();

            // Act
            stopwatch.Start();
            await Task.Delay(100);
            stopwatch.Dispose();
            await Task.Delay(100);

            // Assert
            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.Zero);
#if DEBUG
            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.FromMilliseconds(100));
            stopwatch.Elapsed.Should().BeLessThan(TimeSpan.FromMilliseconds(200));
#endif
        }


        [Fact]
        public async Task ShouldStartUsingStartNew()
        {
            // Arrange

            // Act
            var stopwatch = StopWatch.StartNew();
            await Task.Delay(100);

            // Assert
            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.FromMilliseconds(100));
            stopwatch.Elapsed.Should().BeLessThan(TimeSpan.FromMilliseconds(200));
        }

        [Fact]
        public async Task ShouldStartUsingStartNew_WithCallback()
        {
            // Arrange
            var isCalled = false;
            Action<TimeSpan> callback = ts => { isCalled = true; };

            // Act
            var stopwatch = StopWatch.StartNew(callback);
            await Task.Delay(100);
            stopwatch.Stop();
            await Task.Delay(100);

            // Assert
            isCalled.Should().BeTrue();
            stopwatch.Elapsed.Should().BeGreaterThan(TimeSpan.FromMilliseconds(100));
            stopwatch.Elapsed.Should().BeLessThan(TimeSpan.FromMilliseconds(200));
        }
    }
}
