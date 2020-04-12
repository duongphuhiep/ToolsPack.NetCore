using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ToolsPack.String
{
    /// <summary>
    /// Create XDocument from any object
    /// </summary>
    public static class XDocumentFactory
    {
        public static XDocument CreateDocFromXmlSerializer<T>(T o, XmlSerializer serializer = null, XDocument doc = null)
        {
            if (serializer == null) 
            {
                serializer = new XmlSerializer(typeof(T));
            }
            if (doc == null)
            {
                doc = new XDocument();
            }
            using (var writer = doc.CreateWriter())
            {
                serializer.Serialize(writer, o);
            }
            return doc;
        }
        public static XDocument CreateDocFromDataContractSerializer<T>(T o, DataContractSerializer serializer = null, XDocument doc = null)
        {
            if (serializer == null)
            {
                serializer = new DataContractSerializer(typeof(T));
            }
            return CreateDocFromXmlObjectSerializer(o, serializer, doc);
        }
        public static XDocument CreateDocFromDataContractJsonSerializer<T>(T o, DataContractJsonSerializer serializer = null, XDocument doc = null)
        {
            if (serializer == null)
            {
                serializer = new DataContractJsonSerializer(typeof(T));
            }
            return CreateDocFromXmlObjectSerializer(o, serializer, doc);
        }
        public static XDocument CreateDocFromXmlObjectSerializer<T>(T o, XmlObjectSerializer serializer, XDocument doc = null)
        {
            if (serializer == null)
            {
                serializer = new DataContractSerializer(typeof(T));
            }
            if (doc == null)
            {
                doc = new XDocument();
            }
            using (var writer = doc.CreateWriter())
            {
                serializer.WriteObject(writer, o);
            }
            return doc;
        }
    }
}
