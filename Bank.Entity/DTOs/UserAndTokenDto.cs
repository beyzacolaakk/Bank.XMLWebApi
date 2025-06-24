using Bank.Core.Entities.Abstracts;
using Bank.Core.Utilities.Security.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class UserAndTokenDto : IDto
    {
        public AccessToken Token { get; set; }
    }

}
