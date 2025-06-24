using Bank.Core.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.Entities.Concrete
{
    public class RequestLog : IEntity 
    {
        public int Id { get; set; }

        public string Yontem { get; set; } = string.Empty;

        public string Yol { get; set; } = string.Empty;


        public string SorguParametreleri { get; set; } = string.Empty;


        public string Basliklar { get; set; } = string.Empty;


        public string Govde { get; set; } = string.Empty;

        public int KullaniciId { get; set; }

        public DateTime IstekZamani { get; set; } = DateTime.Now;
    }
}
