using Bank.Core.Entities.Concrete;
using Bank.Core.Utilities.Results;
using Bank.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<List<User>>> GetAll();

        Task<IResult> Add(User user);

        Task<IResult> Update(User user);

        Task<IResult> Delete(User user);

        Task<IDataResult<User>> GetById(int id);

        Task<User> GetByPhone(string phone);

        Task<List<Role>> GetRoles(User user);

        Task<IDataResult<UserInfoDto>> GetUserDetails(int id);
    }

}
