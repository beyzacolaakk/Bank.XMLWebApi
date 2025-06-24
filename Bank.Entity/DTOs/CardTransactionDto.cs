using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class CardTransactionDto : IDto
    {
        public int CardId { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
    }

}
