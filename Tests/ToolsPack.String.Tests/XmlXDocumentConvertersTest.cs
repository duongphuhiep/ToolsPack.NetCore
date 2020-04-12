using N = NLog;
using System;
using Xunit;
using ToolsPack.String;

namespace ToolsPack.String.Tests
{
    public class XmlXDocumentConvertersTest
    {
        private static readonly N.ILogger log = N.LogManager.GetCurrentClassLogger();
        public XmlXDocumentConvertersTest()
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
        public void ConvertTest()
        {
            var p = new paypal() {
                landingPage = "a",
                addrOverride = "b"
            };

            var xmlDoc = XmlDocumentFactory.Create(p);
            var xDoc = XDocumentFactory.CreateDocFromXmlSerializer(p);

            log.Info(xmlDoc.ToXDocument().ToString());
            log.Info(xDoc.ToXmlDocument().OuterXml);
        }
    }
}
