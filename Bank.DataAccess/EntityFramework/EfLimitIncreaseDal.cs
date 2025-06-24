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
    public class EfLimitIncreaseDal : EfEntityRepositoryBase<LimitIncrease, BankContext>, ILimitIncreaseDal
    {
        private readonly BankContext _context;

        public EfLimitIncreaseDal(BankContext context)
        {
            _context = context;
        }

        public async Task<List<LimitIncreaseDto>> GetCardLimitRequests()
        {
            using (var context = new BankContext())
            {
                var result = await (from limitIncrease in context.LimitIncreases
                                    join card in context.Cards on limitIncrease.CardId equals card.Id
                                    join user in context.Users on card.UserId equals user.Id
                                    select new LimitIncreaseDto
                                    {
                                        Id = limitIncrease.Id,
                                        UserId = user.Id,
                                        FullName = user.FullName,
                                        Status = limitIncrease.Status,
                                        ApplicationDate = limitIncrease.ApplicationDate,
                                        CardNumber = card.CardNumber,
                                        CurrentLimit = card.Limit ?? 0,
                                        RequestedLimit = limitIncrease.RequestedLimit
                                    }).ToListAsync();

                return result;
            }
        }

        public async Task<LimitIncreaseDto?> GetCardLimitRequestById(int id)
        {
            using (var context = new BankContext())
            {
                var result = await (from limitIncrease in context.LimitIncreases
                                    join card in context.Cards on limitIncrease.CardId equals card.Id
                                    join user in context.Users on card.UserId equals user.Id
                                    where limitIncrease.Id == id
                                    select new LimitIncreaseDto
                                    {
                                        Id = limitIncrease.Id,
                                        UserId = user.Id,
                                        FullName = user.FullName,
                                        Status = limitIncrease.Status,
                                        ApplicationDate = limitIncrease.ApplicationDate,
                                        CardNumber = card.CardNumber,
                                        CurrentLimit = card.Limit ?? 0,
                                        RequestedLimit = limitIncrease.RequestedLimit
                                    }).FirstOrDefaultAsync();

                return result;
            }
        }

        public async Task<bool> UpdateLimitStatus(int id, string newStatus)
        {
            using (var context = new BankContext())
            {
                var limitRequest = await context.LimitIncreases.FindAsync(id);
                if (limitRequest == null)
                {
                    return false; // Card not found
                }

                limitRequest.Status = newStatus;
                await context.SaveChangesAsync();
                return true;
            }
        }
    }

}
