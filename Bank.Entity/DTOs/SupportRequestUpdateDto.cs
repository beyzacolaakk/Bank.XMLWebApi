using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{
    [XmlRoot("SupportRequest")]
    public class SupportRequestUpdateDto : IDto
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Status")]
        public string? Status { get; set; }

        [XmlElement("Response")]
        public string? Response { get; set; }
    }

}
