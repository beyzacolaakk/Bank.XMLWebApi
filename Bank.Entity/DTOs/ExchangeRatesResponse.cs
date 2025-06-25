using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{
    [XmlRoot("ExchangeRatesResponse")]
    public class ExchangeRatesResponse
    {
        public string BaseCurrency { get; set; }

        public DateTime Date { get; set; }

        [XmlArray("Rates")]
        [XmlArrayItem("Rate")]
        public List<RateItem> Rates { get; set; } = new List<RateItem>();
    }

    public class RateItem
    {
        [XmlAttribute]
        public string Currency { get; set; }

        [XmlText]
        public decimal Value { get; set; }
    }

}
