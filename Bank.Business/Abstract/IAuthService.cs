using Bank.Core.Entities.Concrete;
using Bank.Core.Utilities.Results;
using Bank.Core.Utilities.Security.JWT;
using Bank.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Abstract
{
    public interface IAuthService
    {
        Task<IResult> UserExists(string email);
        Task<IDataResult<AccessToken>> CreateAccessToken(IDataResult<User> user);
        Task<IResult> AddUserRole(IDataResult<User> user);
        Task<IDataResult<UserAndTokenDto>> LoginAndCreateToken(UserLoginDto userLoginDto);

        Task<IResult> Register(UserRegisterDto userRegisterDto);

        void Logout(int id);
    }

}
