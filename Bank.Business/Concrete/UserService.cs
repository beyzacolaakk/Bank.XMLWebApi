using Bank.Business.Abstract;
using Bank.Business.Constant;
using Bank.Core.Entities.Concrete;
using Bank.Core.Utilities.Results;
using Bank.DataAccess.Abstract;
using Bank.Entity.DTOs;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IMemoryCache _memoryCache;

        public UserService(IUserDal userDal, IMemoryCache memoryCache)
        {
            _userDal = userDal;
            _memoryCache = memoryCache;
        }

        public async Task<IResult> Add(User user)
        {
            await _userDal.Add(user);
            return new SuccessResult(Messages.UserAddSuccessful);
        }

        public async Task<IResult> Delete(User user)
        {
            await _userDal.Delete(user);
            return new SuccessResult(Messages.UserDeleteSuccessful);
        }

        public async Task<IResult> Update(User user)
        {
            await _userDal.Update(user);
            return new SuccessResult(Messages.UserUpdateSuccessful);
        }

        public async Task<User> GetByPhone(string phone)
        {
            return await _userDal.Get(u => u.Phone == phone);
        }

        public async Task<IDataResult<List<User>>> GetAll()
        {
            var users = await _userDal.GetAll();
            return new SuccessDataResult<List<User>>(users, "Users retrieved successfully");
        }

        public async Task<IDataResult<User>> GetById(int id)
        {
            var user = await _userDal.Get(u => u.Id == id);
            return new SuccessDataResult<User>(user, "User retrieved successfully");
        }

        public async Task<List<Role>> GetRoles(User user)
        {
            return await _userDal.GetRoles(user);
        }

        public async Task<IDataResult<UserInfoDto>> GetUserDetails(int id)
        {
            string cacheKey = $"user_{id}";

            if (!_memoryCache.TryGetValue(cacheKey, out UserInfoDto cachedData))
            {
                cachedData = await _userDal.GetUser(id);
                _memoryCache.Set(cacheKey, cachedData, TimeSpan.FromMinutes(5));
            }

            return new SuccessDataResult<UserInfoDto>(cachedData, Messages.RetrieveSuccessful);
        }
    }

}
