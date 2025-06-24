using Bank.Core.Entities.Abstracts;
using Bank.Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bank.Entity.Concrete
{
    public class Account : IEntity
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public string AccountNumber { get; set; } = string.Empty;

        public string AccountType { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        public string Currency { get; set; } = "TRY";

        public string? Status { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
