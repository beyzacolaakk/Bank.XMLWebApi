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
    [XmlRoot("Account")]
    public class Account:IEntity
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("UserId")]
        public int UserId { get; set; }

        [XmlElement("AccountNumber")]
        public string AccountNumber { get; set; }

        [XmlElement("AccountType")]
        public string AccountType { get; set; }

        [XmlElement("Balance")]
        public decimal Balance { get; set; }

        [XmlElement("Currency")]
        public string Currency { get; set; } = "TRY";

        [XmlElement("Status")]
        public string? Status { get; set; }

        [XmlElement("CreatedDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
