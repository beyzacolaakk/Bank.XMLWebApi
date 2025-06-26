using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.DTOs
{
    [XmlRoot("Branches")]
    public class BranchesWrapper
    {
        [XmlElement("Branch")]
        public List<BranchDto> Items { get; set; } = new();
    }

    public class BranchDto : IDto
    {
        public int Id { get; set; }
        public string BranchName { get; set; }
    }


}
