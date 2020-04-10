# ToolsPack.Webservice

This package help you to `AddLog` to the webservice client call. Checkout the [ServiceFactoryTests.cs](https://github.com/duongphuhiep/ToolsPack.NetCore/blob/master/Tests/ToolsPack.Webservice.Tests/ServiceFactoryTests.cs)

```CSharp
ILogger log = loggerFactory.CreateLogger<SomeWebServiceClient>(); //create a Microsoft Logger
var svc = new SomeWebServiceClient(...); //create the service 

//tell the service to use the logger to log raw request
svc.AddLog(logger); //shortcut for svc.Endpoint.EndpointBehaviors.Add(new LoggingEndpointBehaviour(new LoggingMessageInspector(logger))); 
```

Raw request are logged on the TRACE level. In order to see them, make sure that the min log level is set to `Trace`
```Csharp
LoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.SetMinimumLevel(LogLevel.Trace);
});
```

## Use Case

1. Generate a WebService consumer (SDK)
```
dotnet tool install --global dotnet-svcutil
dotnet-svcutil http://localhost/someSoapService.asmx?wsdl
```
or use the `svcutil.exe` of the "Developer Command Prompt for Visual Studio"
```
svcutil http://localhost/someSoapService.asmx?wsdl /o:C:\ ... \generated\SomeSoapService.cs /sc /n:*,MyApp.SoapServiceClient
```

2. Create a service client and add the logging behaviour

```CSharp
ILogger log = loggerFactory.CreateLogger<SomeWebServiceClient>(); //create a Microsoft Logger
var svc = new SomeWebServiceClient(...); //create the service 

//tell the service to use the logger to log raw request
svc.AddLog(logger); //shortcut for svc.Endpoint.EndpointBehaviors.Add(new LoggingEndpointBehaviour(new LoggingMessageInspector(logger))); 
```

## `ServiceFactory` is deprecated

```Csharp
SomeSoapService svc = ServiceFactory.CreateServiceProxy<SomeSoapService>(new Uri("https://test-server/someSoapService/Service.asmx"));
var resu = svc.SomeMethodService(new SomeMethodRequest {
    someParam = "Hello",
});
```
Today the `ServiceFactory` is no longer neccessary, I recommend that you create the client directly
```
var svc = new SomeWebServiceClient(...); //create the service 
```
