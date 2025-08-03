using Xunit;
using Xunit.Abstractions;

namespace ToolsPack.String.Tests;

public class XmlXDocumentConvertersTest(ITestOutputHelper _testOutput)
{
    [Fact]
    public void ConvertTest()
    {
        var p = new paypal
        {
            landingPage = "a",
            addrOverride = "b"
        };

        var xmlDoc = XmlDocumentFactory.Create(p);
        var xDoc = XDocumentFactory.CreateDocFromXmlSerializer(p);

        _testOutput.WriteLine(xmlDoc.ToXDocument().ToString());
        _testOutput.WriteLine(xDoc.ToXmlDocument().OuterXml);
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
}