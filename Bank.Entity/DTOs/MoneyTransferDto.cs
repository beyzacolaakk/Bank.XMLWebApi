using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{

    [XmlRoot("MoneyTransferDto")]
    public class MoneyTransferDto : IDto
    {
        [XmlElement("userId")]
        public int UserId { get; set; }

        [XmlElement("senderAccountId")]
        public string SenderAccountId { get; set; } = string.Empty;

        [XmlElement("receiverAccountId")]
        public string ReceiverAccountId { get; set; } = string.Empty;

        [XmlElement("amount")]
        public decimal Amount { get; set; }

        [XmlElement("transactionType")]
        public string TransactionType { get; set; } = string.Empty;

        [XmlElement("description")]
        public string? Description { get; set; }

        [XmlElement("paymentMethod")]
        public string PaymentMethod { get; set; } = string.Empty;
    }


}
