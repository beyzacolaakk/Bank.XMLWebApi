using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.Concrete
{
    public class CardTransaction : IEntity
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public decimal CurrentBalance { get; set; }

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; } = string.Empty;

        public string TransactionType { get; set; } = string.Empty;

        public DateTime TransactionDate { get; set; } = DateTime.Now;

    }

}
