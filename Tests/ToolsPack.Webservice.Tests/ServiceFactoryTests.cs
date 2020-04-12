using Microsoft.VisualStudio.TestTools.UnitTesting;
using N = NLog;
using ToolsPack.Webservice;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using ToolsPack.NLog;
using ServiceReference;

namespace ToolsPack.Webservice.Tests
{
    [TestClass()]
    public class ServiceFactoryTests
    {
        private string webserviceUrl = "http://secure.smartbearsoftware.com/samples/testcomplete14/webservices/Service.asmx?wsdl";
        private N.ILogger nlog = N.LogManager.GetCurrentClassLogger();
        private ILoggerFactory loggerFactory;

        [TestInitialize]
        public void Setup()
        {
            var conf = LogQuickConfig.SetupFileAndConsole("./logs/ServiceFactoryTests.log");
            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddNLog(conf);
                builder.SetMinimumLevel(LogLevel.Trace);
            });
        }
        [TestMethod()]
        public void CreateServiceProxyTest()
        {
            var svc = ServiceFactory.CreateServiceProxy<SampleWebServiceSoap>(new Uri(webserviceUrl));
            Console.WriteLine(svc.GetCurrentTimeAsync().Result);
        }
        [TestMethod()]
        public void LoggingWebserviceTest()
        {
            var log = loggerFactory.CreateLogger<SampleWebServiceSoap>();
            var svc = new SampleWebServiceSoapClient(SampleWebServiceSoapClient.EndpointConfiguration.SampleWebServiceSoap, webserviceUrl);
            svc.AddLog(log, LogLevel.Information);
            log.LogInformation("Here the current time: {CurrentTime}", svc.GetCurrentTimeAsync().Result);
            log.LogInformation("Here the sample object: {SampleObject}", svc.GetSampleObjectAsync(1).Result);
        }

    }
}