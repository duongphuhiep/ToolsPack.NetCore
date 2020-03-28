# ToolsPack.NLog

NLog helper - format the structured logs with `Newtonsoft.Json` as described in the [NLog documentation](https://github.com/NLog/NLog/wiki/How-to-use-structured-logging)

Usage:

```csharp
JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { 
    NullValueHandling = NullValueHandling.Ignore, 
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore, 
    Formatting=Formatting.Indented 
};
NLog.Config.ConfigurationItemFactory.Default.JsonConverter = new ToolsPack.NLog.JsonNetSerializer(jsonSerializerSettings);
```

Note: Here how to use `NLog` to create a `Microsoft.Extensions.Logging.ILogger`: https://stackoverflow.com/a/60859465/347051