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
    public interface IAccountDal : IEntityRepository<Account>
    {
        Task<List<AccountDto>> GetAccountsByUserIdAsync(int userId);

        List<int> GetUserAccountIds(int userId);

        Task<List<AccountRequestDto>> GetAccountRequests();

        Task<RequestCountsDto> GetRequestCounts();

        Task<bool> UpdateAccountStatus(int id, string newStatus);

        Task<AccountRequestDto?> GetAccountRequestById(int id);
    }


}
