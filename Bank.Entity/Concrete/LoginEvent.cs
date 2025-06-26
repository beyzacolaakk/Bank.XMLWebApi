using Bank.Core.Entities.Abstracts;
using Bank.Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Xml.Serialization;
namespace Bank.Entity.Concrete
{


    [XmlRoot("LoginEvent")]
    public class LoginEvent : IEntity
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("userId")]
        public int UserId { get; set; }

        [XmlElement("ipAddress")]
        public string IpAddress { get; set; } = string.Empty;

        [XmlElement("isSuccessful")]
        public bool IsSuccessful { get; set; }

        [XmlElement("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

}
