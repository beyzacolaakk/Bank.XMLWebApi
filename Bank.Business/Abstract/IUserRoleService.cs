using Bank.Core.Entities.Concrete;
using Bank.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Abstract
{
    public interface IUserRoleService
    {
        Task<IDataResult<List<UserRole>>> GetAll();

        Task<IResult> Add(UserRole userRole);

        Task<IResult> Update(UserRole userRole);

        Task<IResult> Delete(UserRole userRole);

        Task<IDataResult<UserRole>> GetById(int id);
    }

}
