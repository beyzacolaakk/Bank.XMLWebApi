using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class SupportRequestUpdateDto : IDto
    {
        public int Id { get; set; }

        public string? Status { get; set; }

        public string? Response { get; set; }
    }

}
