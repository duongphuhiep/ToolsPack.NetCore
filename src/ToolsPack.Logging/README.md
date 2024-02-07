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

```
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

```
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

```
22:56:59,866 [DEBUG] Begin process(val=Lorem ipsum, useCache=true)
22:56:59,970 [INFO ] process - 102350 mcs - step 1
22:57:00,144 [DEBUG] process - 173295 mcs - step 2
22:57:00,259 [INFO ] process - 114036 mcs - Step 3)
22:57:00,452 [INFO ] End process(val=Lorem ipsum, useCache=true) : Total elapsed 585436 mcs
```

## Other benchmark library

* If you want to optimize a particular static (and stateless) function, checkout [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) project. See [video tutorial here](https://www.youtube.com/watch?v=EWmufbVF2A4). (Unlike other project, the `ElapsedTimeLogger` is just a `ILogger` so it fits to be injected to any production application) 
* https://miniprofiler.com/dotnet/
