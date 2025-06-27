using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Bank.Test.Iterative_Test
{
    public static class XmlTestHelper
    {
        public static string SerializeToXml<T>(T obj)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }
        public static bool ValidateXml(string xmlContent, string xsdPath, out List<string> errors)
        {

            var errorList = new List<string>();

            var settings = new XmlReaderSettings();
            settings.Schemas.Add(null, xsdPath);
            settings.ValidationType = ValidationType.Schema;

            settings.ValidationEventHandler += (sender, args) =>
            {
                errorList.Add(args.Message);
            };

            using var stringReader = new StringReader(xmlContent);
            using var reader = XmlReader.Create(stringReader, settings);

            try
            {
                while (reader.Read()) { }
                errors = errorList;
                return errors.Count == 0;
            }
            catch (Exception ex)
            {
                errorList.Add("Exception: " + ex.Message);
                errors = errorList;
                return false;
            }
        }
    }
}
