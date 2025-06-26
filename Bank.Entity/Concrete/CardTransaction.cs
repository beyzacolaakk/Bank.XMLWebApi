using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Xml.Serialization;
namespace Bank.Entity.Concrete
{


    [XmlRoot("CardTransaction")]
    public class CardTransaction : IEntity
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("cardId")]
        public int CardId { get; set; }

        [XmlElement("currentBalance")]
        public decimal CurrentBalance { get; set; }

        [XmlElement("amount")]
        public decimal Amount { get; set; }

        [XmlElement("description")]
        public string? Description { get; set; }

        [XmlElement("status")]
        public string Status { get; set; } = string.Empty;

        [XmlElement("transactionType")]
        public string TransactionType { get; set; } = string.Empty;

        [XmlElement("transactionDate")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }


}
