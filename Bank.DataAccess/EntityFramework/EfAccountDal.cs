using Bank.Core.DataAccess;
using Bank.DataAccess.Abstract;
using Bank.DataAccess.EntityFramework.Context;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.DataAccess.EntityFramework
{
    public class EfAccountDal : EfEntityRepositoryBase<Account, BankContext>, IAccountDal
    {
        public EfAccountDal(BankContext context)
        {
        }

        public async Task<List<AccountDto>> GetAccountsByUserIdAsync(int userId)
        {
            using var context = new BankContext();
            return await context.Accounts
                .Where(a => a.UserId == userId)
                .Select(a => new AccountDto
                {
                    AccountType = a.AccountType,
                    Currency = a.Currency,
                    Balance = a.Balance
                })
                .ToListAsync();
        }

        public async Task<List<AccountRequestDto>> GetAccountRequests()
        {
            using var context = new BankContext();

            var result = await (from account in context.Accounts
                                join user in context.Users
                                on account.UserId equals user.Id
                                select new AccountRequestDto
                                {
                                    FullName = user.FullName,
                                    PhoneNumber = user.Phone,
                                    ApplicationDate = account.CreatedDate,
                                    Status = account.Status,
                                    AccountNumber = account.AccountNumber,
                                    Id = account.Id,
                                    Email = user.Email
                                }).ToListAsync();

            return result;
        }

        public async Task<AccountRequestDto?> GetAccountRequestById(int id)
        {
            using var context = new BankContext();

            var result = await (from account in context.Accounts
                                join user in context.Users
                                on account.UserId equals user.Id
                                where account.Id == id
                                select new AccountRequestDto
                                {
                                    FullName = user.FullName,
                                    PhoneNumber = user.Phone,
                                    ApplicationDate = account.CreatedDate,
                                    Status = account.Status,
                                    AccountNumber = account.AccountNumber,
                                    Id = account.Id,
                                    Email = user.Email
                                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<RequestCountsDto> GetRequestCounts()
        {
            using var context = new BankContext();

            var accountRequests = await context.Accounts.CountAsync(a => a.Status == "Pending");
            var cardRequests = await context.Cards.CountAsync(c => c.Status == "Pending");
            var supportRequestsOpen = await context.SupportRequests.CountAsync(s => s.Status == "Open");
            var supportRequestsInProgress = await context.SupportRequests.CountAsync(s => s.Status == "InProgress");
            var limitIncreaseRequests = await context.LimitIncreases.CountAsync(l => l.Status == "Pending");

            return new RequestCountsDto
            {
                AccountRequests = accountRequests,
                CardRequests = cardRequests,
                SupportRequests = supportRequestsOpen + supportRequestsInProgress,
                LimitIncreaseRequests = limitIncreaseRequests
            };
        }

        public List<int> GetUserAccountIds(int userId)
        {
            using var context = new BankContext();

            return context.Accounts
                .Where(a => a.UserId == userId)
                .Select(a => a.Id)
                .ToList();
        }

        public async Task<bool> UpdateAccountStatus(int id, string newStatus)
        {
            using var context = new BankContext();

            var accountToUpdate = await context.Accounts.FindAsync(id);
            if (accountToUpdate == null)
            {
                return false;
            }

            accountToUpdate.Status = newStatus;
            await context.SaveChangesAsync();
            return true;
        }
    }

}
