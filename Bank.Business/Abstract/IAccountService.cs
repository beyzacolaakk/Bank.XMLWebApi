using Bank.Core.Utilities.Results;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Abstract
{
    public interface IAccountService
    {
        Task<IDataResult<List<Account>>> GetAll();

        Task<IResult> Add(Account account);

        Task<IResult> Update(Account account);

        Task<IResult> Delete(Account account);

        Task<IDataResult<Account>> GetByAccountNumber(string id);

        Task<IDataResult<Account>> GetById(int id);

        Task<IDataResult<decimal>> TransferMoney(string senderAccountId, string receiverAccountId, decimal amount);

        Task<IDataResult<decimal>> WithdrawOrDeposit(DepositWithdrawDto withdrawDepositDto);

        Task<IResult> AutoCreateAccount(CreateAccountDto accountCreateDto); 

        Task<Account> GetUserFirstCheckingAccount(int userId);

        Task<IDataResult<List<Account>>> GetAllByUserId(int id);

        Task<IDataResult<AssetsDto>> GetAssetsAsync(int userId);

        Task<List<int>> GetUserAccountIds(int userId);

        Task<IDataResult<List<AccountRequestDto>>> GetAccountRequests();

        Task<IDataResult<RequestCountsDto>> GetRequestCounts();

        Task<IResult> UpdateAccountStatus(UpdateStatusDto statusUpdateDto);

        Task<IDataResult<AccountRequestDto>> GetAccountRequestById(int id);
    }


}
