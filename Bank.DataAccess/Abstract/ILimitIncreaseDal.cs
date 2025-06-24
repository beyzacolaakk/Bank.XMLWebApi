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
    public interface ILimitIncreaseDal : IEntityRepository<LimitIncrease>
    {
        Task<List<LimitIncreaseDto>> GetCardLimitRequests();

        Task<bool> UpdateLimitStatus(int id, string newStatus);

        Task<LimitIncreaseDto?> GetCardLimitRequestById(int id);
    }

}
