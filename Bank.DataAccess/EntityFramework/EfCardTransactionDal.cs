using Bank.Core.DataAccess;
using Bank.DataAccess.Abstract;
using Bank.DataAccess.EntityFramework.Context;
using Bank.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.DataAccess.EntityFramework
{
    public class EfCardTransactionDal : EfEntityRepositoryBase<CardTransaction, BankContext>, ICardTransactionDal
    {
        public EfCardTransactionDal(BankContext context)
        {
        }

        public List<CardTransaction> GetCardTransactions(List<int> cardIds)
        {
            using (var context = new BankContext())
            {
                return context.CardTransactions
                              .Where(ct => cardIds.Contains(ct.CardId))
                              .ToList();
            }
        }
    }

}
