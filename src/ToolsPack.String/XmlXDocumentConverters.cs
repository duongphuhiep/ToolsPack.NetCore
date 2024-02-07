using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ToolsPack.String
{
    /// <summary>
    /// Convert between (from/to) XmlDocument and XDocument
    /// </summary>
    public static class XmlXDocumentConverters
    {
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            if (xDocument == null) throw new ArgumentNullException(nameof(xDocument));
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }
    }
}
