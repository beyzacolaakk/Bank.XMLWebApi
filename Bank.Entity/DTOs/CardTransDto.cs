using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class CardTransDto 
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
    }

}
