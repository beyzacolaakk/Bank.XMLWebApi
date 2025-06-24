using Bank.Core.DataAccess;
using Bank.Core.Entities.Concrete;
using Bank.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        Task<List<Role>> GetRoles(User user);

        Task<User> AddAndReturnIdAsync(User user);
        Task<UserInfoDto> GetUser(int id);
    }

}
