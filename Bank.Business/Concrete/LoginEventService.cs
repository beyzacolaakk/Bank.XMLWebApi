using Bank.Business.Abstract;
using Bank.Business.Constant;
using Bank.Core.Utilities.Results;
using Bank.DataAccess.Abstract;
using Bank.Entity.Concrete;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Concrete
{
    public class LoginEventService : ILoginEventService
    {
        private readonly ILoginEventDal _loginEventDal;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "LoginEventList";

        public LoginEventService(ILoginEventDal loginEventDal, IMemoryCache memoryCache)
        {
            _loginEventDal = loginEventDal;
            _memoryCache = memoryCache;
        }

        public async Task<IResult> Add(LoginEvent loginEvent)
        {
            await _loginEventDal.Add(loginEvent);
            return new SuccessResult(Messages.AddSuccessful);
        }

        public async Task<IResult> Update(LoginEvent loginEvent)
        {
            await _loginEventDal.Update(loginEvent);
            return new SuccessResult(Messages.UpdateSuccessful);
        }

        public async Task<IResult> Delete(LoginEvent loginEvent)
        {
            await _loginEventDal.Delete(loginEvent);
            return new SuccessResult(Messages.DeleteSuccessful);
        }

        public async Task<IDataResult<List<LoginEvent>>> GetAll(string sortBy = "Time", bool desc = false)
        {
            if (_memoryCache.TryGetValue(CacheKey, out List<LoginEvent> cachedList))
            {
                var sortedList = desc
                    ? cachedList.OrderByDescending(x => x.Timestamp).ToList()
                    : cachedList.OrderBy(x => x.Timestamp).ToList();

                return new SuccessDataResult<List<LoginEvent>>(sortedList, Messages.GetAllSuccessful);
            }

            var list = await _loginEventDal.GetAll();

            _memoryCache.Set(CacheKey, list, TimeSpan.FromMinutes(5));

            var sorted = desc
                ? list.OrderByDescending(x => x.Timestamp).ToList()
                : list.OrderBy(x => x.Timestamp).ToList();

            return new SuccessDataResult<List<LoginEvent>>(sorted, Messages.GetAllSuccessful);
        }
        public async Task<IDataResult<LoginEvent>> GetById(int id)
        {
            var loginEvent = await _loginEventDal.Get(x => x.Id == id);
            return new SuccessDataResult<LoginEvent>(loginEvent, Messages.GetByIdSuccessful);
        }
    }

}
