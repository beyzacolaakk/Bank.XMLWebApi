using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{
    [XmlRoot("LimitIncreaseDto")]
    public class LimitIncreaseDto : IDto
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("UserId")]
        public int UserId { get; set; }

        [XmlElement("FullName")]
        public string FullName { get; set; } = string.Empty;

        [XmlElement("CardNumber")]
        public string CardNumber { get; set; } = string.Empty;

        [XmlElement("CurrentLimit")]
        public decimal? CurrentLimit { get; set; }

        [XmlElement("RequestedLimit")]
        public decimal? RequestedLimit { get; set; }

        [XmlElement("ApplicationDate")]
        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [XmlElement("Status")]
        public string Status { get; set; } = string.Empty;
    }


}
