using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.Concrete
{
    [XmlRoot("SupportRequest")]
    public class SupportRequest : IEntity
    {
        [XmlElement("id")]

        public int Id { get; set; }

        [XmlElement("userId")]
        public int UserId { get; set; }

        [XmlElement("subject")]
        public string? Subject { get; set; }

        [XmlElement("message")]
        public string? Message { get; set; }

        [XmlElement("status")]
        public string Status { get; set; } = "Pending";

        [XmlElement("response")]
        public string? Response { get; set; }

        [XmlElement("category")]
        public string? Category { get; set; }

        [XmlElement("createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }

}
