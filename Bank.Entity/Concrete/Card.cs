using Bank.Core.Entities.Abstracts;
using Bank.Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.Concrete
{
    public class Card : IEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string CardNumber { get; set; } = string.Empty;

        public string CardType { get; set; } = string.Empty;

        public string CVV { get; set; } = string.Empty;

        public decimal? Limit { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string? Status { get; set; }

        public bool IsActive { get; set; } = true;


    }

}
