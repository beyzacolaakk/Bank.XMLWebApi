using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.Concrete
{
    public class SupportRequest : IEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public string? Response { get; set; }

        public string? Category { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }

}
