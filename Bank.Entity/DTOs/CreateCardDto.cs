using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{
    [XmlRoot("CreateCardDto")]
    public class CreateCardDto : IDto
    {
        [XmlElement("userId")]
        public int UserId { get; set; }

        [XmlElement("cardType")]
        public string CardType { get; set; }
    }

}
