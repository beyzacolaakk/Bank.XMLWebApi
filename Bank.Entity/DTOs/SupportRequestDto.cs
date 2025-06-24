using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.DTOs
{
    public class SupportRequestDto : IDto
    {
        public int Id { get; set; }  // Usually assigned by the backend; can be removed if not needed during creation.

        public int UserId { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string? Category { get; set; }

        public string? FullName { get; set; }

        public DateTime? Date { get; set; }  // Typically assigned by the backend.

        public string? Response { get; set; }

        public string? Status { get; set; }
    }

}
