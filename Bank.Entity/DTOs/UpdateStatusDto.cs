using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    using System.Xml.Serialization;

    [XmlRoot("UpdateStatusDto")]
    public class UpdateStatusDto:IDto
    {
        [XmlElement("id")]
        public int? Id { get; set; }

        [XmlElement("status")]
        public string? Status { get; set; }
    }


}
