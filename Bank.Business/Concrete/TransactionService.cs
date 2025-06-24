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
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionDal _transactionDal;
        private readonly IAccountService _accountService;
        private readonly ICardService _cardService;
        private readonly ICardTransactionService _cardTransactionService;

        public TransactionService(
            ITransactionDal transactionDal,
            IAccountService accountService,
            ICardService cardService,
            ICardTransactionService cardTransactionService)
        {
            _transactionDal = transactionDal;
            _accountService = accountService;
            _cardService = cardService;
            _cardTransactionService = cardTransactionService;
        }

        public async Task<IResult> Add(Transaction transaction)
        {
            await _transactionDal.Add(transaction);
            return new SuccessResult(Messages.AddSuccessful);
        }

        public async Task<IResult> Update(Transaction transaction)
        {
            await _transactionDal.Update(transaction);
            return new SuccessResult(Messages.UpdateSuccessful);
        }

        public async Task<IResult> Delete(Transaction transaction)
        {
            await _transactionDal.Delete(transaction);
            return new SuccessResult(Messages.DeleteSuccessful);
        }

        public async Task<IResult> SendMoney(MoneyTransferDto sendMoneyDto)
        {
            var recipientAccountResult = await _accountService.GetByAccountNumber(sendMoneyDto.ReceiverAccountId);
            if (!recipientAccountResult.Success || recipientAccountResult.Data == null)
                return new ErrorResult("Recipient account not found.");

            Account senderAccount = null;
            Card senderCard = null;

            if (sendMoneyDto.PaymentMethod == "account")
            {
                var senderAccountResult = await _accountService.GetByAccountNumber(sendMoneyDto.SenderAccountId);
                if (!senderAccountResult.Success || senderAccountResult.Data == null)
                    return new ErrorResult("Sender account not found.");

                senderAccount = senderAccountResult.Data;
            }
            else
            {
                var senderCardResult = await _cardService.GetByCardNumber(sendMoneyDto.SenderAccountId);
                if (!senderCardResult.Success || senderCardResult.Data == null)
                    return new ErrorResult("Sender card not found.");

                senderCard = senderCardResult.Data;
            }

            var transferResult = await _accountService.TransferMoney(
                sendMoneyDto.SenderAccountId,
                sendMoneyDto.ReceiverAccountId,
                sendMoneyDto.Amount);

            decimal? updatedBalance = (transferResult as IDataResult<decimal>)?.Data;

            if (sendMoneyDto.PaymentMethod == "card")
            {
                var cardTransaction = new CardTransaction
                {
                    Description = sendMoneyDto.Description,
                    Status = transferResult.Success ? "Successful Transfer" : "Failed Transfer",
                    TransactionDate = DateTime.Now,
                    CurrentBalance = updatedBalance!.Value,
                    TransactionType = sendMoneyDto.TransactionType,
                    CardId = senderCard.Id,
                    Amount = sendMoneyDto.Amount
                };

                var cardTransactionResult = await _cardTransactionService.Add(cardTransaction);
                if (!cardTransactionResult.Success)
                    return new ErrorResult(Messages.TransactionNotSaved);
            }

            var transaction = new Transaction
            {
                Description = sendMoneyDto.Description,
                ReceiverAccountId = recipientAccountResult.Data.Id,
                SenderAccountId = senderAccount?.Id,
                CardId = senderCard?.Id,
                TransactionDate = DateTime.Now,
                Amount = sendMoneyDto.Amount,
                TransactionType = sendMoneyDto.TransactionType,
                CurrentBalance = updatedBalance,
                Status = transferResult.Success ? "Successful Transfer" : "Failed Transfer"
            };

            var transactionResult = await Add(transaction);
            if (!transactionResult.Success)
                return new ErrorResult(Messages.TransactionNotSaved);

            return transferResult.Success
                ? new SuccessResult(Messages.MoneyTransferSuccessful)
                : new ErrorResult(Messages.MoneyTransferFailed);
        }

        public async Task<IDataResult<List<TransactionSummaryDto>>> GetLast4CardTransactionsForUser(int userId)
        {
            var accountIds = await Task.Run(() => _accountService.GetUserAccountIds(userId));

            if (!accountIds.Any())
                return new ErrorDataResult<List<TransactionSummaryDto>>(Messages.MoneyTransferFailed);

            var transactions = await Task.Run(() =>
                _transactionDal.GetTransactions(accountIds)
                              .OrderByDescending(t => t.TransactionDate)
                              .Take(4)
                              .ToList());

            var lastTransactions = transactions.Select(t => new TransactionSummaryDto
            {
                Description = t.Description,
                Status = t.Status,
                CurrentBalance = t.CurrentBalance!.Value,
                TransactionType = t.TransactionType,
                Date = t.TransactionDate,
                Amount = t.Amount
            }).ToList();

            return new SuccessDataResult<List<TransactionSummaryDto>>(lastTransactions, Messages.RetrieveSuccessful);
        }

        public async Task<IResult> WithdrawOrDeposit(DepositWithdrawDto withdrawDepositDto)
        {
            IResult transferResult;
            int? senderId = null;

            if (withdrawDepositDto.TransactionType == "card")
            {
                var senderCard = await _cardService.GetByCardNumber(withdrawDepositDto.AccountId);
                if (senderCard == null || senderCard.Data == null)
                {
                    return new ErrorResult("Sender card not found.");
                }

                senderId = senderCard.Data.Id;
                transferResult = await _cardService.DepositWithdraw(withdrawDepositDto);
            }
            else
            {
                var senderAccount = await _accountService.GetByAccountNumber(withdrawDepositDto.AccountId.ToString());
                if (senderAccount == null || senderAccount.Data == null)
                {
                    return new ErrorResult("Sender account not found.");
                }

                senderId = senderAccount.Data.Id;
                transferResult = await _accountService.WithdrawOrDeposit(withdrawDepositDto);
            }

            if (senderId == null || transferResult == null)
            {
                return new ErrorResult("Transaction could not be completed.");
            }

            decimal? updatedBalance = (transferResult as IDataResult<decimal>)?.Data;

            var transaction = new Transaction
            {
                Description = null,
                TransactionDate = DateTime.Now,
                Amount = withdrawDepositDto.Amount,
                CurrentBalance = updatedBalance ?? 0,
                TransactionType = withdrawDepositDto.TransactionType,
                Status = transferResult.Success ? "Successful Transfer" : "Failed Transfer"
            };

            if (withdrawDepositDto.TransactionType == "card")
            {
                transaction.CardId = senderId;
                transaction.SenderAccountId = null;

                var cardTransaction = new CardTransaction
                {
                    Description = null,
                    TransactionDate = DateTime.Now,
                    CardId = senderId.Value,
                    Amount = withdrawDepositDto.Amount,
                    CurrentBalance = updatedBalance ?? 0,
                    TransactionType = withdrawDepositDto.TransactionType,
                    Status = transferResult.Success ? "Successful Transfer" : "Failed Transfer",
                };
                await _cardTransactionService.Add(cardTransaction);
            }
            else
            {
                transaction.SenderAccountId = senderId;
                transaction.CardId = null;
            }

            var transactionResult = await Add(transaction);

            if (!transactionResult.Success)
                return new ErrorResult(Messages.TransactionNotSaved);

            return transferResult.Success
                ? new SuccessResult(Messages.MoneyTransferSuccessful)
                : new ErrorResult(Messages.MoneyTransferFailed);
        }

        public async Task<IDataResult<List<Transaction>>> GetAll()
        {
            var transactions = await _transactionDal.GetAll();
            return new SuccessDataResult<List<Transaction>>(transactions, Messages.GetAllSuccessful);
        }

        public async Task<IDataResult<Transaction>> GetById(int id)
        {
            var transaction = await _transactionDal.Get(t => t.Id == id);
            return new SuccessDataResult<Transaction>(transaction, Messages.GetByIdSuccessful);
        }
    }

}
