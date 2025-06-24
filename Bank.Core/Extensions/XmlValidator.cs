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
            string validationErrors = string.Empty;  // local değişken

            try
            {
                var settings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Parse,
                    ValidationType = ValidationType.DTD
                };

                settings.ValidationEventHandler += (sender, e) =>
                {
                    validationErrors += e.Message + Environment.NewLine;
                };

                using var reader = XmlReader.Create(new StringReader(xmlContent), settings);
                while (reader.Read()) { }

                errors = validationErrors;
                return string.IsNullOrEmpty(validationErrors);
            }
            catch (Exception ex)
            {
                errors = ex.Message;
                return false;
            }
        }




    }

}
