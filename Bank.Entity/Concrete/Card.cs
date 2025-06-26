using Bank.Core.Entities.Abstracts;
using Bank.Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.Concrete
{
    [XmlRoot("Card")]
    public class Card : IEntity
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("userId")]
        public int UserId { get; set; }

        [XmlElement("cardNumber")]
        public string CardNumber { get; set; } = string.Empty;

        [XmlElement("cardType")]
        public string CardType { get; set; } = string.Empty;

        [XmlElement("cvv")]
        public string CVV { get; set; } = string.Empty;

        [XmlElement("limit")]
        public decimal? Limit { get; set; }

        [XmlElement("expirationDate")]
        public DateTime ExpirationDate { get; set; }

        [XmlElement("status")]
        public string? Status { get; set; }

        [XmlElement("isActive")]
        public bool IsActive { get; set; } = true;
    }


}
