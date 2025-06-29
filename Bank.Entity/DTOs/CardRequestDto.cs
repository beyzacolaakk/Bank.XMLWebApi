using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{
    [XmlRoot("CardRequestDto")]
    public class CardRequestDto : IDto
    {
        
    [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("FullName")]
        public string FullName { get; set; }

        [XmlElement("CardType")]
        public string CardType { get; set; }

        [XmlElement("Limit", IsNullable = true)]
        public decimal? Limit { get; set; }
        [XmlElement("ExpirationDate")]
        public DateTime Date { get; set; }
        [XmlElement("Status")]
        public string Status { get; set; }
    }

}
