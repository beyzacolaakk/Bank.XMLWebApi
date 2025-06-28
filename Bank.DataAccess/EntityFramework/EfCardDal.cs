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
    public class EfCardDal : EfEntityRepositoryBase<Card, BankContext>, ICardDal
    {
        private readonly BankContext _context;

        public EfCardDal(BankContext context)
        {
            _context = context;
        }

        public async Task<List<CardDto>> GetCardsByUserIdAsync(int userId)
        {
            using (var context = new BankContext())
            {
                return await context.Cards
                    .Where(c => c.UserId == userId && c.CardType == "Credit")
                    .Select(c => new CardDto
                    {
                        CardNumber = c.CardNumber,
                        Limit = c.Limit
                    })
                    .ToListAsync();
            }
        }

        public List<int> GetUserCardIds(int userId)
        {
            using (var context = new BankContext())
            {
                return context.Cards
                    .Where(c => c.UserId == userId)
                    .Select(c => c.Id)
                    .ToList();
            }
        }

        public async Task<List<CardRequestDto>> GetCardRequests()
        {
            using (var context = new BankContext())
            {
                var result = await (from card in context.Cards
                                    join user in context.Users
                                        on card.UserId equals user.Id
                                    select new CardRequestDto
                                    {
                                        FullName = user.FullName,
                                        Date = DateTime.Now,
                                        Status = card.Status,
                                        CardType = card.CardType,
                                        Id = card.Id,
                                        Limit = card.Limit.HasValue ? card.Limit.Value : 0 // Null check
                                    }).ToListAsync();

                return result;
            }
        }

        public async Task<CardRequestDto?> GetCardRequestById(int id)
        {
            using (var context = new BankContext())
            {
                var result = await (from card in context.Cards
                                    join user in context.Users
                                        on card.UserId equals user.Id
                                    where card.Id == id
                                    select new CardRequestDto
                                    {
                                        FullName = user.FullName,
                                        Date = DateTime.Now, // You may prefer card.CreationDate here
                                        Status = card.Status,
                                        CardType = card.CardType,
                                        Id = card.Id,
                                        Limit = card.Limit ?? 0
                                    }).FirstOrDefaultAsync();

                return result; // Can be null, caller should check
            }
        }

        public async Task<bool> UpdateCardLimit(int cardId, decimal newLimit)
        {
            using (var context = new BankContext())
            {
                var card = await context.Cards.FindAsync(cardId);
                if (card == null)
                {
                    return false; // Card not found
                }

                card.Limit = newLimit;
                await context.SaveChangesAsync();
                return true; // Successfully updated
            }
        }

        public async Task<bool> UpdateCardStatus(int id, string newStatus)
        {
            using (var context = new BankContext())
            {
                var cardToUpdate = await context.Cards.FindAsync(id);
                if (cardToUpdate == null)
                {
                    return false;
                }

                cardToUpdate.Status = newStatus;
                await context.SaveChangesAsync();
                return true;
            }
        }
    }

}
