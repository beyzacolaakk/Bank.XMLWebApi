using Bank.Core.Utilities.Results;
using Bank.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Abstract
{
    public interface ILoginTokenService
    {
        Task<IDataResult<List<LoginToken>>> GetAll();

        Task<IResult> Add(LoginToken loginToken);

        Task<IResult> Update(LoginToken loginToken);

        Task<IResult> Delete(LoginToken loginToken);

        Task<IDataResult<LoginToken>> GetById(int id);
    }

}
