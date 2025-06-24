using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Core.Extensions
{
    [XmlRoot("ErrorDetails")]
    public class ErrorDetails
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public string ToXml()
        {
            var xmlSerializer = new XmlSerializer(typeof(ErrorDetails));
            using var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, this);
            return stringWriter.ToString();
        }
    }

    [XmlRoot("ValidationErrorDetails")]
    public class ValidationErrorDetails : ErrorDetails
    {
        [XmlArray("ValidationFailures")]
        [XmlArrayItem("ValidationFailure")]
        public List<ValidationFailureSerializable> Errors { get; set; }

        public string ToXml()
        {
            var xmlSerializer = new XmlSerializer(typeof(ValidationErrorDetails));
            using var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, this);
            return stringWriter.ToString();
        }
    }

    public class ValidationFailureSerializable
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
