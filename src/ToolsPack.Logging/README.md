# ToolsPack.Logging

## ElapsedTimeLogger

Micro-benchmark a part of code to investigate on performance. This is a normal `ILogger` or more precisely a decorator to a ILogger, when you use it to log a message it will "decorate" the message with the duration (elapsed time) since the previous logged message.

```CSharp
class MyCalculator
{
    private static readonly ILogger Log = loggerFactory.CreateLogger<ElapsedTimeLoggerTests>();

    public void Process()
    {
        using (var etw = ElapsedTimeLogger.Create(Log, "blockCodeName"))
        {
            ...
            etw.LogInformation("step 1");
            ...
            etw.LogDebug("step 2");
            ...
            etw.LogInformation("Step 3)");
            ...
        } //"sum up log" is displayed here
    }
}
```

- The `etw` wrap the usual logger `Log`, we use `etw` to log message instead of the usual `Log`
- the `blockCodeName` is repeated in the start of each log message, so that we can filter log message by "blockCodeName"
- Each log message will display the elapsed time (in micro-second) since the last log message.
- A **sum up log** will display the total elapsed time (in micro-second) when the `etw` object is disposed.

```text
22:56:59,866 [DEBUG] Begin blockCodeName
22:56:59,970 [INFO ] blockCodeName - 102350 mcs - step 1
22:57:00,144 [DEBUG] blockCodeName - 173295 mcs - step 2
22:57:00,259 [INFO ] blockCodeName - 114036 mcs - Step 3)
22:57:00,452 [INFO ] End blockCodeName : Total elapsed 585436 mcs
```

### Auto Jump Log Level

```CSharp
var etw = ElapsedTimeLogger.Create(Log, "checkIntraday").InfoEnd().AutoJump(150, 250).AutoJumpLastLog(500, 1000)
```

- The log level will auto jump to INFO if the elapsed time exceeds 150 ms
- The log level will auto jump to WARN if the elapsed time exceeds 250 ms
- The above **sum up log** will switch to INFO if the total elapsed time exceeds 500 ms
- The above **sum up log** will switch to WARN if the total elapsed time exceeds 1 sec

### Customize Start Context and End context

```CSharp
var etw = ElapsedTimeLogger.Create(Log, "foo", "Start_context", "End_context");
```

will give

```text
22:56:59,866 [DEBUG] Begin Start_context
22:56:59,970 [INFO ] foo - 102350 mcs - step 1
22:57:00,144 [DEBUG] foo - 173295 mcs - step 2
22:57:00,259 [INFO ] foo - 114036 mcs - Step 3)
22:57:00,452 [INFO ] End End_context : Total elapsed 585436 mcs
```

We often display the parameter of the functions in the "Start context". Example:

```CSharp
public void process(string val, bool useCache)
{
    var context = string.Format("process(val={0}, useCache={1})", val, useCache);
    using (var etw = ElapsedTimeLogger.Create(Log, "process", context))
    {
        ...
        etw.LogInformation("step 1");
        ...
        etw.LogDebug("step 2");
        ...
        etw.LogInformation("Step 3)");
        ...
    } //"sum up log" is displayed here
}
```

will give

```text
22:56:59,866 [DEBUG] Begin process(val=Lorem ipsum, useCache=true)
22:56:59,970 [INFO ] process - 102350 mcs - step 1
22:57:00,144 [DEBUG] process - 173295 mcs - step 2
22:57:00,259 [INFO ] process - 114036 mcs - Step 3)
22:57:00,452 [INFO ] End process(val=Lorem ipsum, useCache=true) : Total elapsed 585436 mcs
```

### Other benchmark library

- If you want to optimize a particular static (and stateless) function, checkout [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) project. See [video tutorial here](https://www.youtube.com/watch?v=EWmufbVF2A4). (Unlike other project, the `ElapsedTimeLogger` is just a `ILogger` so it fits to be injected to any production application)
- <https://miniprofiler.com/dotnet/>

## MockLogger

Your test is executing some application codes which log certain messages via the `Microsoft.Extensions.Logging` library.
Once the application codes finished, you want to Assert (or to Verify) that some messages are logged as expected.

The `MockLogger` allows you to "spy" on the log events happening during the application execution. It can be added to your logging pipeline along with others real loggers of your application (Serilog, NLog...)

Checkout the [`MockLoggerTests.cs`](../../tests/ToolsPack.Logging.Tests/MockLoggerTests.cs) for full demonstration.

Here how it works:

1) Use a Mock library (such as Moq, NSubstitue..) to instantiate a `MockLogger`. For eg:

```CSharp
MockLogger _mocklogger = Substitute.For<MockLogger>();
```

2) Ask your application codes to use the `MockLogger` by providing (or by injecting) it to your Application codes, for eg:

```CSharp
var diContainer = new ServiceCollection();
diContainer.AddLogging(builder =>
{
    builder.AddMockLogger(_mocklogger);
    
    //you can apply specific filters for MockLogger
    builder
        .AddFilter<MockLoggerProvider>("System", LogLevel.Error)
        .AddFilter<MockLoggerProvider>("Microsoft", LogLevel.Error)
        .AddFilter<MockLoggerProvider>("App.Metrics", LogLevel.Error);
        
    //or else it will follows the global filters
    builder.AddFilter(null, LogLevel.Trace);
});
```

3) Then use your Mock library to spy (`NSubstitue.Received()`, `Moq.Verify`..) on the `IsLogged` hooks of the `MockLogger`. For eg:

```CSharp
_mocklogger.Received().IsLogged(
    Arg.Is(LogLevel.Error),
    Arg.Any<EventId>(),
    Arg.Is<Exception?>(ex => ex.Message == "some exception"),
    Arg.Is<string>(s => s.Contains("some error on Greeting")));
```

If the mockLogger is unable to capture the log message then check the config (or default config) of the filters.

### Basic usages

```Csharp
//Act
_mocklogger.LogInformation("haha");
_mocklogger.LogError(new Exception("some exception"), "some error on {method}", "Greeting");

//Assert
_mocklogger.Received().IsLogged(
    Arg.Is(LogLevel.Information),
    Arg.Any<EventId>(),
    Arg.Any<Exception?>(),
    Arg.Is("haha"));

_mocklogger.Received().IsLogged(
    Arg.Is(LogLevel.Error),
    Arg.Any<EventId>(),
    Arg.Is<Exception?>(ex => ex.Message == "some exception"),
    Arg.Is<string>(s => s.Contains("some error on Greeting")));

```

### Basic usages for High performance logging

```Csharp
//Logging definition

[LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Send request {message}")]
public static partial void LogSendRequestInfo(this ILogger logger, string message);

[LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "Received something wrong")]
public static partial void LogGetResponseError(this ILogger logger, Exception ex);

//High performance logging

_mocklogger.LogSendRequestInfo("toto");
_mocklogger.LogGetResponseError(new Exception("Failed to do thing"));

//Assert the log content

_mocklogger.Received().IsLogged(
    Arg.Is(LogLevel.Information),
    Arg.Any<EventId>(),
    Arg.Any<Exception?>(),
    Arg.Is("Send request toto"));

_mocklogger.Received().IsLogged(
    Arg.Is(LogLevel.Error),
    Arg.Any<EventId>(),
    Arg.Is<Exception?>(ex => ex.Message == "Failed to do thing"),
    Arg.Is<string>(s => s.Contains("Received something wrong")));
```

### Log structuring

In log structuring, a log item is usually a list of objects (key-value) rather than just a string.

Your application codes might also generate many log items with the same text message but with different objects
in the log structure. In this case, Asserting (verifying) the log text message is not very interesting, you usually want
to spy on the log structure instead.

```Csharp
//Logging definition

[LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "The person {person} odered {order}")]
public static partial void LogStructuringSampleInfo(this ILogger logger, SomePersonObject person, SomeOrderObject order);

//High performance logging, it will generate a structured log which contains 2 objects

_mocklogger.LogStructuringSampleInfo(new SomePersonObject("Hiep", 18), new SomeOrderObject("#1", 30.5));

//Assert the log content

using LogState = IReadOnlyList<KeyValuePair<string, object?>>;

_mocklogger.Received().IsLogged(
    Arg.Is(LogLevel.Information),
    Arg.Any<EventId>(),
    Arg.Any<Exception?>(),
    Arg.Is<string>(msg => Regex.IsMatch(msg, "The person .* odered .*")),
    Arg.Is<LogState>(state => AssertLogStructuring(state)));

//verify that the "person" and "order" objects in the log item got the expected values
private static bool AssertLogStructuring(LogState state)
{
    var person = (from s in state
                    where s.Key == "person"
                    select s.Value as SomePersonObject).First();

    var order = (from s in state
                    where s.Key == "order"
                    select s.Value as SomeOrderObject).First();

    return person.Name == "Hiep" && person.Age == 18 && order.Id == "#1" && order.Amount == 30.5;
}
```

### Use MockLogger to spy on other loggers used by your application codes

You have to add the MockLogger to the logging pipeline of your application, so that it can "monitor" all log events generated by your application codes:

```CSharp
//let's say you have a service provider similar to the one in your ASP.NET app
var diContainer = new ServiceCollection();
diContainer.AddLogging(builder =>
{
    //add the MockLogger to the logging pipeline
    builder.AddMockLogger(_mocklogger);
});
var services = diContainer.BuildServiceProvider();

//let's say your application is using this logger (which is not our MockLogger)
ILogger<MyApp> someAppLogger = services.GetRequiredService<ILogger<MyApp>>();

//your application logged something with this logger
someAppLogger.LogInformation("haha");

//Assert that application logged the expected message whatever the logger is used
_mocklogger.Received().IsLogged(
    Arg.Is(LogLevel.Information),
    Arg.Any<EventId>(),
    Arg.Any<Exception?>(),
    Arg.Is("haha"));
```

### Use MockLogger side by side with other loggers (Serilog for example)

```CSharp
//config serilog
var serilogLogger = new LoggerConfiguration()
    .MinimumLevel.Verbose(); //serilog can see all the log events
    .WriteTo.Seq("http://localhost:5304/")
    .WriteTo.TestOutput(testOutputHelper)
    .WriteTo.File("app.log")
    .CreateLogger()

//create mockLogger with NSubstitute
var mockLogger = Substitute.For<MockLogger>();

//mix Serilog and mockLogger together
var loggerFactory = LoggerFactory.Create(builder => {
    builder
        .AddSerilog(serilogLogger) 
        .AddMockLogger(mockLogger)

        //Make mockLogger receive  all log events, similar to serilog
        .AddFilter(null, LogLevel.Trace); 
});

//Log something:
var logger = loggerFactory.CreateLogger();
logger.LogInformation("hello");
```