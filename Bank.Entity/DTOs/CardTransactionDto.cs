using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{
    [XmlRoot("CardTransactionDto")]
    public class CardTransactionDto : IDto
    {
        [XmlElement("cardId")]
        public int CardId { get; set; }

        [XmlElement("amount")]
        public decimal Amount { get; set; }

        [XmlElement("userId")]
        public int UserId { get; set; }
    }

}
