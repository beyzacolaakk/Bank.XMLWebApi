using Bank.Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.Utilities.Logger
{
    public interface IRequestLogService
    {
        Task RequestLogAsync(RequestLog log);  
    }
}
