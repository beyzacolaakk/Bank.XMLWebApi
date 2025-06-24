using Bank.Core.DataAccess;
using Bank.Core.Utilities.Results;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.DataAccess.Abstract
{
    public interface ICardTransactionDal : IEntityRepository<CardTransaction>
    {
        List<CardTransaction> GetCardTransactions(List<int> cardIds);
    }


}
