using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.Concrete
{
    public class Transaction : IEntity
    {
        public int Id { get; set; }

        public int? SenderAccountId { get; set; }

        public int? ReceiverAccountId { get; set; }

        public int? CardId { get; set; }

        public decimal Amount { get; set; }

        public decimal? CurrentBalance { get; set; }

        public string TransactionType { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;

    }

}
