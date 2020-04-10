using N = NLog;
using System;
using Xunit;

namespace ToolsPack.String.Tests
{
    public class XmlDocumentFactoryTest
    {
        private static readonly N.ILogger log = N.LogManager.GetCurrentClassLogger();
        public XmlDocumentFactoryTest()
        {
            NLog.LogQuickConfig.SetupFileAndConsole("./logs/strings.log");
        }

        public class paypal
        {
            public string landingPage { get; set; }
            public string addrOverride { get; set; }
            public string invoiceId { get; set; }
            public string dupFlag { get; set; }
            public string dupDesc { get; set; }
            public string dupCustom { get; set; }
            public string dupType { get; set; }
            public string mobile { get; set; }
            public string orderDescription { get; set; }
        }
        [Fact]
        public void CreateTest()
        {
            var p = new paypal() {
                landingPage = "a",
                addrOverride = "b"
            };

            var xml = XmlDocumentFactory.Create(p);
            
            log.Info(xml.OuterXml);
        }
    }
}
