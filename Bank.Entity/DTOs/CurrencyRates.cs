using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{
    [XmlRoot("channel")]
    public class CurrencyRates
    {
        [XmlElement("item")]
        public List<CurrencyItem> Items { get; set; }
    }

    public class CurrencyItem
    {
        [XmlElement("targetCurrency")]
        public string Code { get; set; }

        [XmlElement("exchangeRate")]
        public decimal Rate { get; set; }
    }
}
