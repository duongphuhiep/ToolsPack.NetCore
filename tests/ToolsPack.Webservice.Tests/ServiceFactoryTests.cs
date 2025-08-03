using System;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceReference;

namespace ToolsPack.Webservice.Tests;

[TestClass]
public class ServiceFactoryTests
{
    private ILoggerFactory loggerFactory;

    private readonly string webserviceUrl =
        "http://secure.smartbearsoftware.com/samples/testcomplete14/webservices/Service.asmx?wsdl";

    [TestInitialize]
    public void Setup()
    {
        loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddConsole();
        });
    }

    [TestMethod]
    [Obsolete]
    public void CreateServiceProxyTest()
    {
        var svc = ServiceFactory.CreateServiceProxy<SampleWebServiceSoap>(new Uri(webserviceUrl));
        Console.WriteLine(svc.GetCurrentTimeAsync().Result);
    }

    [TestMethod]
    public void LoggingWebserviceTest()
    {
        var log = loggerFactory.CreateLogger<SampleWebServiceSoap>();
        using (var svc = new SampleWebServiceSoapClient(
                   SampleWebServiceSoapClient.EndpointConfiguration.SampleWebServiceSoap, webserviceUrl))
        {
            svc.AddLog(log, LogLevel.Information);
            log.LogInformation("Here the current time: {CurrentTime}", svc.GetCurrentTimeAsync().Result);
            log.LogInformation("Here the sample object: {SampleObject}", svc.GetSampleObjectAsync(1).Result);
        }
    }
}