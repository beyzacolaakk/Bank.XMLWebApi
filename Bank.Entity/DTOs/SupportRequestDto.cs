using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{
    [XmlRoot("SupportRequestDto")]
    public class SupportRequestDto
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("userId")]
        public int UserId { get; set; }

        [XmlElement("subject")]
        public string Subject { get; set; } = string.Empty;

        [XmlElement("message")]
        public string Message { get; set; } = string.Empty;

        [XmlElement("status")]
        public string? Status { get; set; }

        [XmlElement("response")]
        public string? Response { get; set; }

        [XmlElement("category")]
        public string? Category { get; set; }

        [XmlElement("fullName")]
        public string? FullName { get; set; }

        [XmlElement("date")]
        public DateTime? Date { get; set; }
    }

}
