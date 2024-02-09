# ToolsPack.Log4net

Checkout also the `ToolsPack.NLog` package. I personally think that `NLog` is better than `Log4Net`..

## LogQuickConfig

In a Unit test project, or a temporary console application, you donnot have to configure the log4net.config any more.

Call

```CSharp
LogQuickConfig.SetupConsole();
```

or

```CSharp
LogQuickConfig.SetupFile("my_small_app.log");
```

it will setup a typical log4net appender so that you can use them in your test application. Example:

```CSharp
[TestClass]
public class ArrayDisplayerTests
{
    private static readonly ILog Log = LogManager.GetLogger(typeof (ArrayDisplayerTests));

    [ClassInitialize]
    public static void SetUp(TestContext testContext)
    {
        Log4NetQuickSetup.SetUpConsole();
        //or Log4NetQuickSetup.SetUpFile("mytest.log");
    }

    [TestMethod]
    public void DisplayTest()
    {
        Log.Info("it will display to the Console");
        //or to the file if you use SetUpFile()
    }
}
```

See also [code-snippet to quickly configure log4net in a C# project](https://github.com/duongphuhiep/ToolsPack.Net/wiki/log4net)

## ElapsedTimeWatcher <== Deprecated, use the `ToolsPack.Logging.ElapsedTimeLogger` instead

Micro-benchmark a part of code to investigate on performance

```CSharp
class MyCalculator
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(MyCalculator));

    public void Process()
    {
        using (var etw = ElapsedTimeWatcher.Create(Log, "blockCodeName"))
        {
            ...
            etw.Info("step 1");
            ...
            etw.Debug("step 2");
            ...
            etw.Info("Step 3)");
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
var etw = ElapsedTimeWatcher.Create(Log, "checkIntraday").InfoEnd().AutoJump(150, 250).AutoJumpLastLog(500, 1000)
```

- The log level will auto jump to INFO if the elapsed time exceeds 150 ms
- The log level will auto jump to WARN if the elapsed time exceeds 250 ms
- The above **sum up log** will switch to INFO if the total elapsed time exceeds 500 ms
- The above **sum up log** will switch to WARN if the total elapsed time exceeds 1 sec

### Customize Start Context and End context

```CSharp
var etw = ElapsedTimeWatcher.Create(Log, "foo", "Start_context", "End_context");
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
    using (var etw = ElapsedTimeWatcher.Create(Log, "process", context))
    {
        ...
        etw.Info("step 1");
        ...
        etw.DebugFormat("step 2");
        ...
        etw.Info("Step 3)");
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
