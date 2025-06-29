using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Core.Utilities.XMLSerializeToXML
{
    public static class XmlHelper
    {
        public static string SerializeToXml<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));

            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, "http://example.com/account"); 

            using var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, obj, ns);
            return stringWriter.ToString();
        }


    }
}
