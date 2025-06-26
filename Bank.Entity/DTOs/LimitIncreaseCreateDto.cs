using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{
    [XmlRoot("LimitIncreaseCreateDto")]
    public class LimitIncreaseCreateDto : IDto
    {
        [XmlElement("currentLimit")]
        public decimal? CurrentLimit { get; set; }

        [XmlElement("requestedLimit")]
        public decimal RequestedLimit { get; set; }

        [XmlElement("cardNumber")]
        public string? CardNumber { get; set; }

        [XmlElement("status")]
        public string? Status { get; set; }

        [XmlElement("id")]
        public int? Id { get; set; }
    }

}
