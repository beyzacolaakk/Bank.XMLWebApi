using Bank.Business.Abstract;
using Bank.Business.Constant;
using Bank.Core.Utilities.Results;
using Bank.DataAccess.Abstract;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Concrete
{
    public class LimitIncreaseService : ILimitIncreaseService
    {
        private readonly ILimitIncreaseDal _limitIncreaseDal;
        private readonly ICardService _cardService;
        private readonly IMemoryCache _memoryCache;

        public LimitIncreaseService(ILimitIncreaseDal limitIncreaseDal, ICardService cardService, IMemoryCache memoryCache)
        {
            _limitIncreaseDal = limitIncreaseDal;
            _cardService = cardService;
            _memoryCache = memoryCache;
        }

        public async Task<IResult> Add(LimitIncrease limitIncrease)
        {
            _memoryCache.Remove("cardLimitRequestsCache");
            await _limitIncreaseDal.Add(limitIncrease);
            return new SuccessResult(Messages.AddSuccessful);
        }

        public async Task<IResult> AddLimitIncreaseRequest(LimitIncreaseRequestDto limitIncreaseRequest)
        {
            _memoryCache.Remove("cardLimitRequestsCache");
            var cardData = await _cardService.GetById(limitIncreaseRequest.CardId);
            var entity = new LimitIncrease
            {
                ApplicationDate = DateTime.Now,
                Status = "Pending",
                CurrentLimit = limitIncreaseRequest.CurrentLimit,
                CardId = cardData.Data.Id,
                RequestedLimit = limitIncreaseRequest.NewLimit,
            };
            await _limitIncreaseDal.Add(entity);
            return new SuccessResult(Messages.AddSuccessful);
        }

        public async Task<IResult> Update(LimitIncrease limitIncrease)
        {
            _memoryCache.Remove("cardLimitRequestsCache");
            await _limitIncreaseDal.Update(limitIncrease);
            return new SuccessResult(Messages.UpdateSuccessful);
        }

        public async Task<IResult> Delete(int id)
        {
            _memoryCache.Remove("cardLimitRequestsCache");
            var limitIncrease = (await GetById(id)).Data;
            await _limitIncreaseDal.Delete(limitIncrease);
            return new SuccessResult(Messages.DeleteSuccessful);
        }

        public async Task<IDataResult<List<LimitIncrease>>> GetAll()
        {
            var list = await _limitIncreaseDal.GetAll();
            return new SuccessDataResult<List<LimitIncrease>>(list, Messages.GetAllSuccessful);
        }

        public async Task<IDataResult<LimitIncrease>> GetById(int id)
        {
            var entity = await _limitIncreaseDal.Get(x => x.Id == id);
            return new SuccessDataResult<LimitIncrease>(entity, Messages.GetByIdSuccessful);
        }

        public async Task<IDataResult<List<LimitIncreaseDto>>> GetCardLimitRequests()
        {
            var cacheKey = "cardLimitRequestsCache";
            var cachedData = _memoryCache.Get<List<LimitIncreaseDto>>(cacheKey);

            if (cachedData != null)
            {
                return new SuccessDataResult<List<LimitIncreaseDto>>(cachedData, Messages.GetAllSuccessful);
            }

            var list = await _limitIncreaseDal.GetCardLimitRequests();
            _memoryCache.Set(cacheKey, list, TimeSpan.FromMinutes(5));

            return new SuccessDataResult<List<LimitIncreaseDto>>(list, Messages.GetAllSuccessful);
        }

        public async Task<IDataResult<LimitIncreaseDto>> GetCardLimitRequestById(int id)
        {
            var entity = await _limitIncreaseDal.GetCardLimitRequestById(id);
            return new SuccessDataResult<LimitIncreaseDto>(entity, Messages.GetByIdSuccessful);
        }

        public async Task<IResult> UpdateCardLimitRequest(LimitIncreaseCreateDto limitIncreaseAddDto)
        {
            _memoryCache.Remove("cardLimitRequestsCache");

            if (limitIncreaseAddDto.Status == "Active")
            {
                decimal currentLimit = Convert.ToDecimal(limitIncreaseAddDto.CurrentLimit);

                decimal roundedLimit = Math.Ceiling(currentLimit / 5000) * 5000;
                limitIncreaseAddDto.RequestedLimit = limitIncreaseAddDto.RequestedLimit - (roundedLimit - (decimal)limitIncreaseAddDto.CurrentLimit);

                var card = await _cardService.GetByCardNumber(limitIncreaseAddDto.CardNumber!);
                var updateResult = await _cardService.UpdateCardLimit(card.Data.Id, limitIncreaseAddDto.RequestedLimit);

                if (updateResult.Data)
                {
                    var updateStatus = await _limitIncreaseDal.UpdateLimitStatus(limitIncreaseAddDto.Id!.Value, limitIncreaseAddDto.Status!);
                    if (updateStatus)
                    {
                        return new SuccessResult(Messages.UpdateSuccessful);
                    }
                    else
                    {
                        return new ErrorResult(Messages.UpdateFailed);
                    }
                }
                else
                {
                    return new ErrorResult("Limit update failed.");
                }
            }
            else if (limitIncreaseAddDto.Status == "Rejected")
            {
                var updateStatus = await _limitIncreaseDal.UpdateLimitStatus(limitIncreaseAddDto.Id!.Value, limitIncreaseAddDto.Status!);
                if (updateStatus)
                {
                    return new SuccessResult(Messages.UpdateSuccessful);
                }
                else
                {
                    return new ErrorResult(Messages.UpdateFailed);
                }
            }
            else
            {
                return new ErrorResult(Messages.UpdateFailed);
            }
        }
    }
}
