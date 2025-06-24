using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class AssetsDto : IDto
    {
        public decimal? TotalBalance { get; set; }

        public decimal? TotalDebt { get; set; }

        public List<AccountDto> Accounts { get; set; } = new();

        public List<CardDto> Cards { get; set; } = new();
    }

}
