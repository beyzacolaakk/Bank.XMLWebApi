using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    using System.Xml.Serialization;

    public class UserRegisterDto : IDto
    {
        [XmlElement("fullName")]
        public string FullName { get; set; } = string.Empty;

        [XmlElement("email")]
        public string Email { get; set; } = string.Empty;

        [XmlElement("phone")]
        public string Phone { get; set; } = string.Empty;

        [XmlElement("password")]
        public string Password { get; set; } = string.Empty;

        [XmlElement("branch")]
        public int Branch { get; set; }

        [XmlElement("isActive")]
        public bool IsActive { get; set; } = true;
    }


}
