using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class UserLoginDto : IDto
    {
        public string Phone { get; set; }

        public string Password { get; set; }

        public string IpAddress { get; set; }
    }

}
