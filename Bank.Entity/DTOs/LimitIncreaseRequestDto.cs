using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class LimitIncreaseRequestDto : IDto
    {
        public int CardId { get; set; }

        public decimal CurrentLimit { get; set; }

        public decimal NewLimit { get; set; }
    }

}
