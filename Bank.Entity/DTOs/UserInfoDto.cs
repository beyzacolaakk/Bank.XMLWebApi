using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{
    [XmlRoot("UserInfoDto")]
    public class UserInfoDto : IDto
    {
        [XmlElement("fullName")]
        public string FullName { get; set; } = string.Empty;

        [XmlElement("email")]
        public string Email { get; set; } = string.Empty;

        [XmlElement("phone")]
        public string Phone { get; set; } = string.Empty;

        [XmlElement("branch")]
        public string Branch { get; set; } = string.Empty;
    }

}
