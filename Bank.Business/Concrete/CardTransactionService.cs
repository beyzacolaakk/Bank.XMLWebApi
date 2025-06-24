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
    public class CardTransactionService : ICardTransactionService
    {
        private readonly ICardTransactionDal _cardTransactionDal;
        private readonly ICardService _cardService;
        private readonly IAccountService _accountService;

        public CardTransactionService(ICardTransactionDal cardTransactionDal, IAccountService accountService, ICardService cardService)
        {
            _cardTransactionDal = cardTransactionDal;
            _accountService = accountService;
            _cardService = cardService;
        }

        public async Task<IResult> Add(CardTransaction cardTransaction)
        {
            await _cardTransactionDal.Add(cardTransaction);
            return new SuccessResult(Messages.AddSuccessful);
        }

        public async Task<IResult> Update(CardTransaction cardTransaction)
        {
            await _cardTransactionDal.Update(cardTransaction);
            return new SuccessResult(Messages.UpdateSuccessful);
        }

        public async Task<IDataResult<List<CardTransaction>>> GetAll()
        {
            var data = await _cardTransactionDal.GetAll();
            return new SuccessDataResult<List<CardTransaction>>(data, Messages.GetAllSuccessful);
        }

        public async Task<IResult> PerformTransactionWithCard(CardTransactionDto cardTransactionDto)
        {
            if (cardTransactionDto.Amount <= 0)
                return new ErrorResult("Transaction amount must be greater than zero.");

            var cardResult = await _cardService.GetById(cardTransactionDto.CardId);
            if (cardResult == null || cardResult.Data == null)
                return new ErrorResult("Card not found.");

            var card = cardResult.Data;

            switch (card.CardType)
            {
                case "Credit Card":
                    if (card.Limit == null)
                        return new ErrorResult("Credit card limit is not defined.");

                    if (card.Limit < cardTransactionDto.Amount)
                        return new ErrorResult("Insufficient credit card limit.");

                    card.Limit -= cardTransactionDto.Amount;
                    var creditCardUpdate = await _cardService.Update(card);
                    if (!creditCardUpdate.Success)
                        return new ErrorResult("Error occurred while updating credit card.");

                    return new SuccessResult("Credit card transaction successful.");

                case "Debit Card":
                    var account = await _accountService.GetUserFirstCheckingAccount(card.UserId);
                    if (account == null)
                        return new ErrorResult("No bank account found for the user.");

                    if (account.Balance < cardTransactionDto.Amount)
                        return new ErrorResult("Insufficient account balance.");

                    account.Balance -= cardTransactionDto.Amount;
                    var accountUpdate = await _accountService.Update(account);
                    if (!accountUpdate.Success)
                        return new ErrorResult("Error occurred while updating account.");

                    return new SuccessResult("Debit card transaction successful.");

                default:
                    return new ErrorResult("Invalid card type.");
            }
        }

        public async Task<IDataResult<CardTransaction>> GetById(int id)
        {
            var data = await _cardTransactionDal.Get(c => c.Id == id);
            return new SuccessDataResult<CardTransaction>(data, Messages.GetByIdSuccessful);
        }

        public async Task<IDataResult<List<TransactionSummaryDto>>> GetLast4CardTransactionsForUser(int userId)
        {
            var cardIds = await Task.Run(() => _cardService.GetCardIdsByUserId(userId));

            if (!cardIds.Any())
                return new ErrorDataResult<List<TransactionSummaryDto>>();

            var data = await Task.Run(() =>
                _cardTransactionDal.GetCardTransactions(cardIds)
                                  .OrderByDescending(i => i.TransactionDate)
                                  .Take(4)
                                  .ToList());

            var lastTransactions = data.Select(i => new TransactionSummaryDto
            {
                Description = i.Description,
                Status = i.Status,
                CurrentBalance = i.CurrentBalance,
                TransactionType = i.TransactionType,
                Date = i.TransactionDate,
                Amount = i.Amount,
            }).ToList();

            return new SuccessDataResult<List<TransactionSummaryDto>>(lastTransactions);
        }

        public async Task<IResult> Delete(CardTransaction cardTransaction)
        {
            await _cardTransactionDal.Delete(cardTransaction);
            return new SuccessResult(Messages.DeleteSuccessful);
        }
    }

}
