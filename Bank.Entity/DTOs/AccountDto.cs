using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class AccountDto : IDto
    {
        public string AccountType { get; set; } = string.Empty;
        public string Currency { get; set; } = "TL";
        public decimal Balance { get; set; }
    }

}
