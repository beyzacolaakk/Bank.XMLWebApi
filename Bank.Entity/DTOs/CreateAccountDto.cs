using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class CreateAccountDto : IDto
    {
        public int UserId { get; set; }

        public string AccountType { get; set; } = string.Empty;

        public string Currency { get; set; } = "TL";
    }

}
