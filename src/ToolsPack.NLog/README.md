# ToolsPack.NLog

NLog helper - format the structured logs with `Newtonsoft.Json` as described in the [NLog documentation](https://github.com/NLog/NLog/wiki/How-to-use-structured-logging)

Usage:

```csharp
JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { 
    NullValueHandling = NullValueHandling.Ignore, 
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore, 
    Formatting=Formatting.Indented 
};
NLog.Config.ConfigurationItemFactory.Default.JsonConverter = new ToolsPack.NLog.NewtonsoftJsonSerializer(jsonSerializerSettings);
```

Shortcut:

```csharp
LogQuickConfig.UseNewtonsoftJson()
```

Update:

You can also use the new `System.Text.Json` serializer.

```csharp
LogQuickConfig.UseMicrosoftJson()
```

In unit test and for most basic logging needs:

```csharp
LogQuickConfig.SetupConsole();
```

or

```csharp
LogQuickConfig.SetupFile("./log/app.log");
```

or

```csharp
LogQuickConfig.SetupFileAndConsole("./log/app.log");
```

once setup, you can write logs as usual:

```csharp
private static readonly NLog.ILogger log = NLog.LogManager.GetCurrentClassLogger();
log.Info("{@request}", req); //structured logging

```

Some library (APS.NET Core) require a `Microsoft.Extensions.Logging.ILogger` for logging.
You can use `NLog` to create one: <https://stackoverflow.com/a/60859465/347051>

```csharp
private static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddNLog(LogQuickConfig.SetupFileAndConsole("./log/app.log"));
    //LogQuickConfig.UseMicrosoftJson();
});
private static readonly ILogger<Program> log = loggerFactory.CreateLogger<Program>();

log.LogInformation("{@request}", jsonRequest);
```
