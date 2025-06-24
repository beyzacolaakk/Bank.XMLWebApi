using Bank.Core.Entities.Concrete;
using Bank.Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.DataAccess.EntityFramework.Context
{
    public class BankContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-DNUIALQ\SQLKOD;Database=BankXMLDb;integrated Security=true;TrustServerCertificate=True;");
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardTransaction> CardTransactions { get; set; }
        public DbSet<LoginEvent> LoginEvents { get; set; }
        public DbSet<LoginToken> LoginTokens { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<SupportRequest> SupportRequests { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<LimitIncrease> LimitIncreases { get; set; }
        public DbSet<RequestLog> RequestLogs { get; set; }

 
    }
}
