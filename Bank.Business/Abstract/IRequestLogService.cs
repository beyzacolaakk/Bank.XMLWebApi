﻿using Bank.Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Abstract
{
    public interface IRequestLogService
    {
        Task LogRequestAsync(RequestLog log);
    }

}
