using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{
    [XmlRoot("LimitIncreaseRequestDto")]
    public class LimitIncreaseRequestDto : IDto
    {
        [XmlElement("cardId")]
        public int CardId { get; set; }

        [XmlElement("currentLimit")]
        public decimal CurrentLimit { get; set; }

        [XmlElement("newLimit")]
        public decimal NewLimit { get; set; }
    }

}
