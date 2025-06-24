using Bank.Core.Utilities.Results;
using Bank.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Abstract
{
    public interface ILoginEventService
    {
        Task<IDataResult<List<LoginEvent>>> GetAll(string sortBy = "Time", bool desc = false);

        Task<IResult> Add(LoginEvent loginEvent);

        Task<IResult> Update(LoginEvent loginEvent);

        Task<IResult> Delete(LoginEvent loginEvent);

        Task<IDataResult<LoginEvent>> GetById(int id);
    }

}
