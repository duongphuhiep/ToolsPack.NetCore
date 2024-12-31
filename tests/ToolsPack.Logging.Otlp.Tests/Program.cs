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
        options.AddProcessor(new SimpleLogRecordExportProcessor(
            new JsonConsoleLogsExporter(eOpts =>
            {
                eOpts.JsonSerializerOptions.IndentSize = 2;
                eOpts.JsonSerializerOptions.WriteIndented = true;
            })
        ));
    })
);
var logger = loggerFactory.CreateLogger("G");

using (activitySource.StartActivity("hiep activity"))
{
    using (logger.BeginScope(new List<KeyValuePair<string, object?>>
           {
               new("sk1", "sv1"),
               new("sk2", "sv2"),
               new("sk3", new HttpClient())
           }))
    {
        using (logger.BeginScope(new List<KeyValuePair<string, object?>>
               {
                   new("sk11", "sv11"),
                   new("sk21", "sv21"),
                   new("obscureKey", new object())
               }))
        {
            using (logger.BeginScope("simpleScope"))
            {
                logger.LogError(new Exception("bi loi roi"), "Hello {k1} {k2} {k3}", "v1", "v2", new string[] { });
            }
        }
    }
}