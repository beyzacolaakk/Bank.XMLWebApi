using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class MoneyTransferDto : IDto
    {
        public int UserId { get; set; }

        public string SenderAccountId { get; set; } = string.Empty;

        public string ReceiverAccountId { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string TransactionType { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;
    }

}
