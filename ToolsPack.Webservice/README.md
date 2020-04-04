#ToolsPack.Webservice

The `ServiceFactory` help you to create a SOAP WebService accessor instance.

For example:

1. You generated a WebService consumer (SDK) with the `svcutil.exe` of the "Developer Command Prompt for Visual Studio"
```
svcutil http://localhost/someSoapService.asmx?wsdl /o:C:\ ... \generated\SomeSoapService.cs /sc /n:*,MyApp.SoapServiceClient
```

2. Then you can use the `ServiceFactory` to use the generated codes right away:
```
var svc = ServiceFactory<SomeSoapService>.GetServiceProxy(new Uri("https://test-server/someSoapService/Service.asmx"));
var resu = svc.SomeMethodService(new SomeMethodRequest {
    someParam = "Hello",
});
```
