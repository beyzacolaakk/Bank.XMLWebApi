using Bank.Business.Abstract;
using Bank.Core.Entities.Concrete;
using Bank.DataAccess.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Concrete
{
    public class RequestLogService : IRequestLogService
    {
        private readonly BankContext _dbContext;

        public RequestLogService(BankContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LogRequestAsync(RequestLog log)
        {
            _dbContext.RequestLogs.Add(log);
            await _dbContext.SaveChangesAsync();
        }
    }

}
