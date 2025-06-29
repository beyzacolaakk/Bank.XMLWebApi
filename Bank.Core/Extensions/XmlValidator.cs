using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace Bank.Core.Extensions
{
    public static class XmlValidator
    {
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
        public static bool ValidateXmlWithDtd(string xmlContent, string dtdPath, out string errors)
        {
            var validationErrors = new StringBuilder();

            try
            {
                string dtdFileName = Path.GetFileName(dtdPath); 
                string dtdDirectory = Path.GetDirectoryName(dtdPath)!;

                string xmlWithDoctype = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<!DOCTYPE Transaction SYSTEM \"{dtdFileName}\">\n{xmlContent}";

                var settings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Parse,
                    ValidationType = ValidationType.DTD,
                    XmlResolver = new XmlUrlResolver()
                };

                settings.ValidationEventHandler += (sender, e) =>
                {
                    validationErrors.AppendLine(e.Message);
                };

                var baseUri = "file:///" + dtdDirectory.Replace("\\", "/") + "/";
                var context = new XmlParserContext(null, null, null, null, null, null, null, null, XmlSpace.None, Encoding.UTF8)
                {
                    BaseURI = baseUri
                };

                using var reader = XmlReader.Create(new StringReader(xmlWithDoctype), settings, context);

                while (reader.Read()) { }

                errors = validationErrors.ToString();
                return string.IsNullOrWhiteSpace(errors);
            }
            catch (Exception ex)
            {
                errors = ex.Message;
                return false;
            }
        }






    }

}
