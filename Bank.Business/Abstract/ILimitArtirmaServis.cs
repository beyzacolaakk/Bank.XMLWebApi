using Bank.Core.Utilities.Results;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Abstract
{
    public interface ILimitIncreaseService
    {
        Task<IDataResult<List<LimitIncrease>>> GetAll();

        Task<IResult> Add(LimitIncrease limitIncrease);

        Task<IResult> Update(LimitIncrease limitIncrease);

        Task<IResult> Delete(int id);

        Task<IDataResult<LimitIncrease>> GetById(int id);

        Task<IDataResult<List<LimitIncreaseDto>>> GetCardLimitRequests();

        Task<IResult> AddLimitIncreaseRequest(LimitIncreaseRequestDto limitIncreaseRequest);

        Task<IResult> UpdateCardLimitRequest(LimitIncreaseCreateDto limitIncreaseAddDto);

        Task<IDataResult<LimitIncreaseDto>> GetCardLimitRequestById(int id);
    }

}
