using Bank.Core.Entities.Abstracts;
using Bank.Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entity.Concrete
{
    public class LoginEvent : IEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string IpAddress { get; set; } = string.Empty;

        public bool IsSuccessful { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
