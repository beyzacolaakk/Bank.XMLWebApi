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
    public interface ICardTransactionService
    {
        Task<IDataResult<List<CardTransaction>>> GetAll();

        Task<IResult> Add(CardTransaction cardTransaction);

        Task<IResult> Update(CardTransaction cardTransaction);

        Task<IResult> Delete(CardTransaction cardTransaction);

        Task<IDataResult<CardTransaction>> GetById(int id);

        Task<IResult> PerformTransactionWithCard(CardTransactionDto cardTransactionDto);

        Task<IDataResult<List<TransactionSummaryDto>>> GetLast4CardTransactionsForUser(int userId);
    }

}
