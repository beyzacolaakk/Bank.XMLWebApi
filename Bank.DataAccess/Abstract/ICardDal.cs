using Bank.Core.DataAccess;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.DataAccess.Abstract
{
    public interface ICardDal : IEntityRepository<Card>
    {
        Task<List<CardDto>> GetCardsByUserIdAsync(int userId);

        List<int> GetUserCardIds(int userId);

        Task<List<CardRequestDto>> GetCardRequests();

        Task<bool> UpdateCardLimit(int cardId, decimal newLimit);

        Task<bool> UpdateCardStatus(int id, string newStatus);

        Task<CardRequestDto?> GetCardRequestById(int id);
    }

}
