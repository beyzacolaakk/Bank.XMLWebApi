using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.Entities.Concrete
{
    public class User : IEntity
    {

        public int Id { get; set; }

        public string FullName { get; set; } 

        public string Email { get; set; }

        public string Phone { get; set; } 

        public byte[] PasswordSalt { get; set; }

        public byte[] PasswordHash { get; set; }

        public int BranchId { get; set; }  

        public DateTime RegistrationDate { get; set; }

        public bool Active  {  get; set; } 

    }
}
