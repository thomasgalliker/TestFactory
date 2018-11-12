# TestFactory
[![Version](https://img.shields.io/nuget/v/TestFactory.svg)](https://www.nuget.org/packages/TestFactory)  [![Downloads](https://img.shields.io/nuget/dt/TestFactory.svg)](https://www.nuget.org/packages/TestFactory)

<img src="https://raw.githubusercontent.com/thomasgalliker/TestFactory/master/logo.png" width="100" height="100" alt="TestFactory" align="right"></img>

TestFactory is a utility which helps composing and orchestrating test runs. TestFactory allows to create collections of test steps and guarantees that test steps run in a specific order. The result of a test run is summarized in a test summary.

### Download and Install TestFactory
This library is available on NuGet: https://www.nuget.org/packages/TestFactory/
Use the following command to install TestFactory using NuGet package manager console:

    PM> Install-Package TestFactory

You can use this library in any .Net project which is compatible to PCL (e.g. Xamarin Android, iOS, Windows Phone, Windows Store, Universal Apps, etc.)

### API Usage
#### Orchestrating Test Sequences
The following code excerpt shows how TestFactory can be used to orchestrate a collection of independent test steps:
```C#
// Setup test steps
var systemTestBuilder = new SystemTestBuilder()
    .AddTestStep(() => { })
    .AddTestStep(() => { throw new Exception("Something failed"); });

// Run test suite
var testResult = await systemTestBuilder.Run();

// Assert test result
testResult.IsSuccessful.Should().BeFalse();
```

#### Reporting Test Runs
The following code excerpt shows how TestFactory reports the test result summary of a test run:
```
Test Result Summary:
--------------------

Overall success: False
Overall duration: 00:00:00.0005243
Number of test steps: 2 (successful: 1 / failed: 1)

-> ActionTestStep:			IsSuccessful = True,			Duration = 00:00:00.0005243
-> ActionTestStep:			IsSuccessful = False,			Duration = 
        System.Exception: Something failed
           at TestFactory.Tests.SystemTestBuilderTests.<>c.<ShouldPartiallyFailIfLastStepFails>b__2_1() in C:\src\github\thomasgalliker\TestFactory\Tests\TestFactory.Tests\SystemTestBuilderTests.cs:line 24
           at TestFactory.TestSteps.ActionTestStep.Run() in C:\src\github\thomasgalliker\TestFactory\TestFactory\TestSteps\ActionTestStep.cs:line 16
           at TestFactory.SystemTestBuilder.Run() in C:\src\github\thomasgalliker\TestFactory\TestFactory\SystemTestBuilder.cs:line 32
```


#### Running Test Steps In Parallel
TestFactory handles sequential and parallel execution. Following code snippet demonstrates how sequential and parallel test steps can be used in one test run.
Step 1 and 2 are executed in sequence while the following five tasks run in parallel to eachother. The execution roughly takes 3 seconds.
```C#
// Arrange
var systemTestBuilder = new SystemTestBuilder()
        .AddTestStepAsync(async () => { await Task.Delay(1000); this.testOutputHelper.WriteLine($"Step1 (sync) finished on Thread {Thread.CurrentThread.ManagedThreadId}"); })
        .AddTestStepAsync(async () => { await Task.Delay(1000); this.testOutputHelper.WriteLine($"Step2 (sync) finished on Thread {Thread.CurrentThread.ManagedThreadId}"); })
        .AddParallelTestStep(
            async () => { await Task.Delay(1000); this.testOutputHelper.WriteLine($"Step3 (async) finished on Thread {Thread.CurrentThread.ManagedThreadId}"); },
            async () => { await Task.Delay(1000); this.testOutputHelper.WriteLine($"Step4 (async) finished on Thread {Thread.CurrentThread.ManagedThreadId}"); },
            async () => { await Task.Delay(1000); this.testOutputHelper.WriteLine($"Step5 (async) finished on Thread {Thread.CurrentThread.ManagedThreadId}"); },
            async () => { await Task.Delay(1000); this.testOutputHelper.WriteLine($"Step6 (async) finished on Thread {Thread.CurrentThread.ManagedThreadId}"); },
            async () => { await Task.Delay(1000); this.testOutputHelper.WriteLine($"Step7 (async) finished on Thread {Thread.CurrentThread.ManagedThreadId}"); });

// Act
var testResult = await systemTestBuilder.Run();

// Assert
testResult.IsSuccessful.Should().BeTrue();
this.testOutputHelper.WriteLine(testResult.ToString());
```

The console output (in this case xunit's ITestOutputHelper) displays following order of execution. We observe, that the steps 3 to 7 are executed in random order, parallel to eachother.
```
Step1 (sync) finished on Thread 12
Step2 (sync) finished on Thread 9
Step4 (async) finished on Thread 9
Step3 (async) finished on Thread 9
Step7 (async) finished on Thread 9
Step6 (async) finished on Thread 9
Step5 (async) finished on Thread 9
```

The test result summarizes the test run das follows:
```C#
Test Result Summary:
--------------------

Overall success: True
Overall duration: 00:00:03.0295196
Number of test steps: 3 (successful: 3 / failed: 0)

-> TaskTestStep:			IsSuccessful = True,			Duration = 00:00:01.0116610

-> TaskTestStep:			IsSuccessful = True,			Duration = 00:00:01.0017726

-> ParallelTestStep:			IsSuccessful = True,			Duration = 00:00:01.0160860
        |-> TaskTestStep:			IsSuccessful = True,			Duration = 00:00:01.0037856
        |-> TaskTestStep:			IsSuccessful = True,			Duration = 00:00:01.0000253
        |-> TaskTestStep:			IsSuccessful = True,			Duration = 00:00:01.0099737
        |-> TaskTestStep:			IsSuccessful = True,			Duration = 00:00:01.0085763
        |-> TaskTestStep:			IsSuccessful = True,			Duration = 00:00:01.0070593

```



### License
This project is Copyright &copy; 2018 [Thomas Galliker](https://ch.linkedin.com/in/thomasgalliker). Free for non-commercial use. For commercial use please contact the author.
