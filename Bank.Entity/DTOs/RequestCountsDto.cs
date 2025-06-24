using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class RequestCountsDto : IDto
    {
        public int AccountRequests { get; set; }
        public int CardRequests { get; set; }
        public int SupportRequests { get; set; }
        public int? LimitIncreaseRequests { get; set; }
    }

}
