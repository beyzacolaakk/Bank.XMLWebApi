using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{

    [XmlRoot("CardPartial")]
    public class CardPartialDto
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("CardType")]
        public string CardType { get; set; }

        [XmlElement("Limit")]
        public decimal? Limit { get; set; }

        [XmlElement("Status")]
        public string Status { get; set; }

        [XmlElement("ExpirationDate")]
        public DateTime ExpirationDate { get; set; }
    }
}
