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
    public class EfLoginEventDal : EfEntityRepositoryBase<LoginEvent, BankContext>, ILoginEventDal
    {
    }

}
