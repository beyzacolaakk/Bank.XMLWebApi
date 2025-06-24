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
    public interface ITransactionService
    {
        Task<IDataResult<List<Transaction>>> GetAll();

        Task<IResult> Add(Transaction transaction);

        Task<IResult> Update(Transaction transaction);

        Task<IResult> Delete(Transaction transaction);

        Task<IDataResult<Transaction>> GetById(int id);

        Task<IResult> SendMoney(MoneyTransferDto sendMoneyDto);

        Task<IResult> WithdrawOrDeposit(DepositWithdrawDto withdrawDepositDto);

        Task<IDataResult<List<TransactionSummaryDto>>> GetLast4CardTransactionsForUser(int userId);
    }

}
