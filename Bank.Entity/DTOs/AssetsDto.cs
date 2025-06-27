using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{
    [XmlRoot("AssetsDto")]
    public class AssetsDto : IDto
    {
        [XmlElement("totalBalance")]
        public decimal? TotalBalance { get; set; }

        [XmlElement("totalDebt")]
        public decimal? TotalDebt { get; set; }

        [XmlArray("accounts")]
        [XmlArrayItem("account")]
        public List<AccountDto> Accounts { get; set; } = new();

        [XmlArray("cards")]
        [XmlArrayItem("card")]
        public List<CardDto> Cards { get; set; } = new();
    }

}
