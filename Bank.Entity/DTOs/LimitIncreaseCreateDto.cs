using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class LimitIncreaseCreateDto : IDto
    {
        public decimal? CurrentLimit { get; set; }

        public decimal RequestedLimit { get; set; }

        public string? CardNumber { get; set; }

        public string? Status { get; set; }

        public int? Id { get; set; }
    }

}
