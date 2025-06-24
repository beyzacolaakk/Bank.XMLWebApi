using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class DepositWithdrawDto
    {
        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public string TransactionType { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string AccountId { get; set; } = string.Empty;

        public string OperationType { get; set; } = string.Empty;
    }

}
