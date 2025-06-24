using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class CardDto : IDto
    {
        public string CardNumber { get; set; } = string.Empty;

        public decimal? Limit { get; set; }
    }

}
