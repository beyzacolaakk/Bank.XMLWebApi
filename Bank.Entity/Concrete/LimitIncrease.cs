using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.Concrete
{
    public class LimitIncrease : IEntity
    {
        public int Id { get; set; }

        public decimal CurrentLimit { get; set; }

        public decimal RequestedLimit { get; set; }

        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = string.Empty;

        public int CardId { get; set; }
    }

}
