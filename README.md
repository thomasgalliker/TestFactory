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
#### Orchestrating Test Steps
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

### License
This project is Copyright &copy; 2018 [Thomas Galliker](https://ch.linkedin.com/in/thomasgalliker). Free for non-commercial use. For commercial use please contact the author.
