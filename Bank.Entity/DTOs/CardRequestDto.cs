using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class CardRequestDto : IDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string CardType { get; set; }
        public decimal? Limit { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }

}
