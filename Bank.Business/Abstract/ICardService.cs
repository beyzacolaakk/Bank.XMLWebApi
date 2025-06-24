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
    public interface ICardService
    {
        Task<IDataResult<List<Card>>> GetAll();

        Task<IResult> Add(Card card);

        Task<IResult> Update(Card card);

        Task<IResult> Delete(Card card);

        Task<IDataResult<Card>> GetById(int id);

        Task<IResult> AutoCreateCard(CreateCardDto createCardDto);

        Task<IDataResult<List<Card>>> GetAllById(int id);

        Task<IDataResult<decimal>> DepositWithdraw(DepositWithdrawDto depositWithdrawDto);

        Task<IDataResult<List<CardDto>>> GetCardsByUserIdAsync(int userId);

        Task<IDataResult<Card>> GetByCardNumber(string cardNumber);

        Task<List<int>> GetCardIdsByUserId(int userId);

        Task<IDataResult<List<CardRequestDto>>> GetCardRequests();

        Task<IDataResult<bool>> UpdateCardLimit(int cardId, decimal newLimit);

        Task<IResult> UpdateCardStatus(UpdateStatusDto updateStatusDto);

        Task<IDataResult<CardRequestDto>> GetCardRequestById(int id);
    }

}
