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
    public class EfTransactionDal : EfEntityRepositoryBase<Transaction, BankContext>, ITransactionDal
    {
        public List<Transaction> GetTransactions(List<int> accountIds)
        {
            using (var context = new BankContext())
            {
                return context.Transactions
                              .Where(t => t.SenderAccountId != null && accountIds.Contains(t.SenderAccountId.Value))
                              .ToList();
            }
        }
    }

}
