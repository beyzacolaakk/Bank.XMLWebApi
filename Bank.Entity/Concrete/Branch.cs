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
    [XmlRoot("Branch")]
    public class Branch : IEntity
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("branchName")]
        public string BranchName { get; set; }

        [XmlElement("address")]
        public string Address { get; set; }

        [XmlElement("phone")]
        public string Phone { get; set; }
    }


}
