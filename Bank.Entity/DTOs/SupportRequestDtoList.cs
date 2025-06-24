using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    using System.Xml.Serialization;

    [XmlRoot("SupportRequests")]
    public class SupportRequestDtoList
    {
        [XmlElement("SupportRequest")]
        public List<SupportRequestDto> Items { get; set; }
    }

}
