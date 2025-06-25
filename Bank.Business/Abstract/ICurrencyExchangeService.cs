using Bank.Core.Utilities.Results;
using Bank.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Abstract
{
    public interface ICurrencyExchangeService
    {
        Task<IDataResult<ExchangeRatesResponse>> GetExchangeRatesAsync();
    }
}
