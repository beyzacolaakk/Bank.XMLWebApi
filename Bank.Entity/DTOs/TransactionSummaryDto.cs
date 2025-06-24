using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class TransactionSummaryDto : IDto
    {
        public decimal CurrentBalance { get; set; }

        public decimal Amount { get; set; }

        public string TransactionType { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; } = string.Empty;
    }

}
