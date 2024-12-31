# ToolsPack.Logging.Oltp

In the current .NET OLTP (v1.10.0) implementation, the [
`LogRecord` class](https://github.com/open-telemetry/opentelemetry-dotnet/blob/86c1d8c/src/OpenTelemetry/Logs/LogRecord.cs)
is not serializable.
It contains a lot of interesting OpenTelemetry information, mixing with other logics.
IMO, actual implementation of this class should rather be called `LogRecordKitchenSink` than `LogRecord`.

This nuget library provides a mapping of the `LogRecord` "kitchen sink" object to a serializable [
`LogRecordDto`](./LogRecordDto.cs) object:
Checkout the [`LogRecordConverter`](./LogRecordConverter.cs) class.
It should facilitate you to develop your own custom exporter (File, Console...).

The library also provides simple implementation of [Console](JsonConsoleLogsExporter.cs)
and [File exporter](./JsonFileLogsExporter.cs) which will format the telemetry log records to json.

## Log to Console

```C#
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ToolsPack.Logging.Otlp;

var resourceBuilder = ResourceBuilder.CreateDefault().AddService("my-service");
ActivitySource activitySource = new("LogScopeTests", "1.0.0");

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(resourceBuilder)
    .AddSource("LogScopeTests")
    .Build();

var loggerFactory = LoggerFactory.Create(builder =>
    builder.AddOpenTelemetry(options =>
    {
        options.SetResourceBuilder(resourceBuilder);
        options.IncludeScopes = true;
        options.IncludeFormattedMessage = true;
        options.AddProcessor(new SimpleLogRecordExportProcessor /*or BatchLogRecordExportProcessor*/(
            new JsonConsoleLogsExporter(exporterOptions =>
            {
                exporterOptions.JsonSerializerOptions.IndentSize = 2;
                exporterOptions.JsonSerializerOptions.WriteIndented = true;
            })
        ));
    })
);
var logger = loggerFactory.CreateLogger("G");
using (activitySource.StartActivity())
{
    logger.LogInformation("Hello {thing}!", "World");
}
/*
{
  "timestamp": "2024-12-31T20:43:40.8521675Z",
  "traceId": "f8c7dc7c4fee9f0dfb2a1c2a42a08c84",
  "spanId": "52e69b4fb2fbde5f",
  "traceFlags": 1,
  "categoryName": "G",
  "severity": 2,
  "formattedMessage": "Hello World!",
  "eventId": {
    "id": 0
  },
  "body": "Hello {thing}!",
  "attributes": {
    "thing": "World",
    "{OriginalFormat}": "Hello {thing}!"
  },
  "scopeValues": [],
  "resource": {
    "service.name": "my-service",
    "service.instance.id": "c983b8ce-cdf4-4cb0-8da6-b30ba6192ca6",
    "telemetry.sdk.name": "opentelemetry",
    "telemetry.sdk.language": "dotnet",
    "telemetry.sdk.version": "1.10.0"
  }
}

*/
```

## Log to file

```C#
using Microsoft.Extensions.Logging;
using OpenTelemetry;

var loggerFactory = LoggerFactory.Create(builder =>
    builder.AddOpenTelemetry(options =>
    {
        options.AddProcessor(new SimpleLogRecordExportProcessor /*or BatchLogRecordExportProcessor*/(
            new JsonFileLogsExporter(exporterOptions => { 
                exporterOptions.FilePath = "./app.log"; 
            })
        ));
    })
);
var logger = loggerFactory.CreateLogger("G");
logger.LogInformation("Hello World");
```