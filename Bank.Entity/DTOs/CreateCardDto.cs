using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class CreateCardDto : IDto
    {
        public int UserId { get; set; }
        public string CardType { get; set; }
    }

}
