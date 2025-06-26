using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.Concrete
{
    [XmlRoot("Transaction")]
    public class Transaction : IEntity
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("senderAccountId")]
        public int? SenderAccountId { get; set; }

        [XmlElement("receiverAccountId")]
        public int? ReceiverAccountId { get; set; }

        [XmlElement("cardId")]
        public int? CardId { get; set; }

        [XmlElement("amount")]
        public decimal Amount { get; set; }

        [XmlElement("currentBalance")]
        public decimal? CurrentBalance { get; set; }

        [XmlElement("transactionType")]
        public string TransactionType { get; set; } = string.Empty;

        [XmlElement("status")]
        public string Status { get; set; } = string.Empty;

        [XmlElement("description")]
        public string? Description { get; set; }

        [XmlElement("transactionDate")]
        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }

}
