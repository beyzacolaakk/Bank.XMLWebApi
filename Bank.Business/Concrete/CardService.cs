using Bank.Business.Abstract;
using Bank.Business.Constant;
using Bank.Core.Utilities.Results;
using Bank.DataAccess.Abstract;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Concrete
{
    public class CardService : ICardService
    {
        private readonly ICardDal _cardDal;

        public CardService(ICardDal cardDal)
        {
            _cardDal = cardDal;
        }

        public async Task<IDataResult<List<CardRequestDto>>> GetCardRequests()
        {
            var cardRequests = await _cardDal.GetCardRequests();
            return new SuccessDataResult<List<CardRequestDto>>(cardRequests, Messages.RetrieveSuccessful);
        }

        public async Task<IResult> Add(Card card)
        {
            await _cardDal.Add(card);
            return new SuccessResult(Messages.AddSuccessful);
        }

        public async Task<IResult> Update(Card card)
        {
            await _cardDal.Update(card);
            return new SuccessResult(Messages.UpdateSuccessful);
        }

        public async Task<IResult> AutoCreateCard(CreateCardDto createCardDto)
        {
            var card = new Card
            {
                UserId = createCardDto.UserId,
                CardNumber = GenerateCardNumber(),
                CVV = GenerateCvv(),
                CardType = createCardDto.CardType,
                ExpirationDate = DateTime.UtcNow.AddYears(3),
                Limit = createCardDto.CardType == "Credit" ? 5000 : (int?)null,
                Status = "Pending",
            };

            await _cardDal.Add(card);
            return new SuccessResult("Card created automatically.");
        }

        public async Task<IDataResult<List<Card>>> GetAll()
        {
            var data = await _cardDal.GetAll();
            return new SuccessDataResult<List<Card>>(data, Messages.RetrieveSuccessful);
        }

        public async Task<IDataResult<CardRequestDto>> GetCardRequestById(int id)
        {
            var cardRequest = await _cardDal.GetCardRequestById(id);
            return new SuccessDataResult<CardRequestDto>(cardRequest, Messages.RetrieveSuccessful);
        }

        public async Task<IDataResult<Card>> GetById(int id)
        {
            var data = await _cardDal.Get(c => c.Id == id);
            return new SuccessDataResult<Card>(data, Messages.RetrieveSuccessful);
        }

        public async Task<IDataResult<Card>> GetByCardNumber(string cardNumber)
        {
            var data = await _cardDal.Get(c => c.CardNumber == cardNumber);
            return new SuccessDataResult<Card>(data, Messages.RetrieveSuccessful);
        }

        public async Task<List<int>> GetCardIdsByUserId(int userId)
        {
            return new List<int>(_cardDal.GetUserCardIds(userId));
        }

        public async Task<IDataResult<List<Card>>> GetAllById(int id)
        {
            var data = await _cardDal.GetAll(c => c.UserId == id && c.Status != "Pending" && c.Status != "Rejected");
            return new SuccessDataResult<List<Card>>(data, Messages.RetrieveSuccessful);
        }

        public async Task<IResult> Delete(Card card)
        {
            await _cardDal.Delete(card);
            return new SuccessResult(Messages.DeleteSuccessful);
        }

        private string GenerateCardNumber()
        {
            var random = new Random();
            // Generate a 16-digit random card number
            return string.Concat(Enumerable.Range(0, 16).Select(_ => random.Next(0, 10).ToString()));
        }

        private string GenerateCvv()
        {
            var random = new Random();
            // Generate a 3-digit CVV
            return random.Next(100, 1000).ToString();
        }

        public async Task<IDataResult<List<CardDto>>> GetCardsByUserIdAsync(int userId)
        {
            var data = await _cardDal.GetCardsByUserIdAsync(userId);
            return new SuccessDataResult<List<CardDto>>(data, Messages.RetrieveSuccessful);
        }

        public async Task<IDataResult<bool>> UpdateCardLimit(int cardId, decimal newLimit)
        {
            var result = await _cardDal.UpdateCardLimit(cardId, newLimit);
            return new SuccessDataResult<bool>(result);
        }

        public async Task<IDataResult<decimal>> DepositWithdraw(DepositWithdrawDto depositWithdrawDto)
        {
            var senderTask = GetByCardNumber(depositWithdrawDto.AccountId.ToString());

            var sender = senderTask.Result;

            if (!sender.Success || sender.Data == null)
                return new ErrorDataResult<decimal>("Sender account not found.");

            try
            {
                if (depositWithdrawDto.TransactionType == "Withdrawal")
                {
                    if (sender.Data.Limit < depositWithdrawDto.Amount)
                        return new ErrorDataResult<decimal>("Insufficient balance in sender account.");
                    sender.Data.Limit -= depositWithdrawDto.Amount;
                }
                else if (depositWithdrawDto.TransactionType == "Deposit")
                {
                    sender.Data.Limit += depositWithdrawDto.Amount;
                }

                await _cardDal.Update(sender.Data);

                return new SuccessDataResult<decimal>(sender.Data.Limit.Value, "Money transfer successful.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<decimal>($"An error occurred during transfer: {ex.Message}");
            }
        }

        public async Task<IResult> UpdateCardStatus(UpdateStatusDto updateStatusDto)
        {
            var result = await _cardDal.UpdateCardStatus(updateStatusDto.Id!.Value, updateStatusDto.Status!);
            if (result)
            {
                return new SuccessResult(Messages.UpdateSuccessful);
            }
            else
            {
                return new ErrorResult(Messages.UpdateFailed);
            }
        }
    }

}
