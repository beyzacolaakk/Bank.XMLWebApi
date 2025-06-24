using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class LimitIncreaseDto : IDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string CardNumber { get; set; } = string.Empty;

        public decimal? CurrentLimit { get; set; }

        public decimal? RequestedLimit { get; set; }

        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = string.Empty;
    }

}
