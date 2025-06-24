using Bank.Core.DataAccess;
using Bank.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.DataAccess.Abstract
{
    public interface ILoginTokenDal : IEntityRepository<LoginToken>
    {
    }

}
