using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    using System.Xml.Serialization;

    [XmlRoot("DepositWithdrawDto")]
    public class DepositWithdrawDto
    {
        [XmlElement("userId")]
        public int UserId { get; set; }

        [XmlElement("amount")]
        public decimal Amount { get; set; }

        [XmlElement("transactionType")]
        public string TransactionType { get; set; } = string.Empty;

        [XmlElement("description")]
        public string? Description { get; set; }

        [XmlElement("accountId")]
        public string AccountId { get; set; } = string.Empty;

        [XmlElement("operationType")]
        public string OperationType { get; set; } = string.Empty;
    }


}
