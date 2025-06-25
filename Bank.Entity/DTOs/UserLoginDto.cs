using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Bank.Entity.DTOs
{

    [XmlRoot("UserLoginDto")]
    public class UserLoginDto
    {
        [XmlElement("phone")]
        public string Phone { get; set; }

        [XmlElement("password")]
        public string Password { get; set; }

        [XmlElement("ipAddress")]
        public string IpAddress { get; set; }
    }


}
