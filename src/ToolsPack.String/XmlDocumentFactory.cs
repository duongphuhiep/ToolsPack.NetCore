using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace ToolsPack.String
{
    /// <summary>
    /// Create a XmlDocument from any object
    /// </summary>
    public static class XmlDocumentFactory
    {
        public static XmlDocument Create<T>(T o)
        {
            return Create(o, new XmlDocument(), new XmlSerializer(typeof(T)));
        }
        public static XmlDocument Create<T>(T o, string defaultNamespace)
        {
            return Create(o, new XmlDocument(), new XmlSerializer(typeof(T), defaultNamespace));
        }
        public static XmlDocument Create<T>(T o, Type[] extraTypes)
        {
            return Create(o, new XmlDocument(), new XmlSerializer(typeof(T), extraTypes));
        }
        public static XmlDocument Create<T>(T o, XmlRootAttribute root)
        {
            return Create(o, new XmlDocument(), new XmlSerializer(typeof(T), root));
        }
        public static XmlDocument Create<T>(T o, XmlAttributeOverrides overrides, Type[] extraTypes, XmlRootAttribute root, string defaultNamespace)
        {
            return Create(o, new XmlDocument(), new XmlSerializer(typeof(T), overrides, extraTypes, root, defaultNamespace));
        }
        public static XmlDocument Create<T>(T o, XmlAttributeOverrides overrides, Type[] extraTypes, XmlRootAttribute root, string defaultNamespace, string location)
        {
            return Create(o, new XmlDocument(), new XmlSerializer(typeof(T), overrides, extraTypes, root, defaultNamespace, location));
        }
        public static XmlDocument Create<T>(T o, NameTable nt)
        {
            return Create(o, new XmlDocument(nt), new XmlSerializer(typeof(T)));
        }
        public static XmlDocument Create<T>(T o, string defaultNamespace, NameTable nt)
        {
            return Create(o, new XmlDocument(nt), new XmlSerializer(typeof(T), defaultNamespace));
        }
        public static XmlDocument Create<T>(T o, Type[] extraTypes, NameTable nt)
        {
            return Create(o, new XmlDocument(nt), new XmlSerializer(typeof(T), extraTypes));
        }
        public static XmlDocument Create<T>(T o, XmlRootAttribute root, NameTable nt)
        {
            return Create(o, new XmlDocument(nt), new XmlSerializer(typeof(T), root));
        }
        public static XmlDocument Create<T>(T o, XmlAttributeOverrides overrides, Type[] extraTypes, XmlRootAttribute root, string defaultNamespace, NameTable nt)
        {
            return Create(o, new XmlDocument(nt), new XmlSerializer(typeof(T), overrides, extraTypes, root, defaultNamespace));
        }
        public static XmlDocument Create<T>(T o, XmlAttributeOverrides overrides, Type[] extraTypes, XmlRootAttribute root, string defaultNamespace, string location, NameTable nt)
        {
            return Create(o, new XmlDocument(nt), new XmlSerializer(typeof(T), overrides, extraTypes, root, defaultNamespace, location));
        }

        private static XmlDocument Create<T>(T o, XmlDocument doc, XmlSerializer serializer)
        {
            XPathNavigator nav = doc.CreateNavigator();
            using (XmlWriter w = nav.AppendChild())
            {
                serializer.Serialize(w, o);
                return doc;
            }
        }
    }
}
