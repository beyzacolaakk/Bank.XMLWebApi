using Bank.Core.Entities.Abstracts;
using Bank.Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.Concrete
{
    [XmlRoot("LoginToken")]
    public class LoginToken : IEntity
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("userId")]
        public int UserId { get; set; }

        [XmlElement("token")]
        public string Token { get; set; } = string.Empty;

        [XmlElement("createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [XmlElement("expirationDate")]
        public DateTime ExpirationDate { get; set; }
    }

}
