using Bank.Core.DataAccess;
using Bank.Core.Entities.Concrete;
using Bank.DataAccess.Abstract;
using Bank.DataAccess.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.DataAccess.EntityFramework
{
    public class EfUserRoleDal : EfEntityRepositoryBase<UserRole, BankContext>, IUserRoleDal
    {
    }

}
