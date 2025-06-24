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
    public class AccountService : IAccountService
    {
        private readonly IAccountDal _accountDal;
        private readonly ICardService _cardService;

        public AccountService(IAccountDal accountDal, ICardService cardService)
        {
            _accountDal = accountDal;
            _cardService = cardService;
        }

        public async Task<IDataResult<List<AccountRequestDto>>> GetAccountRequests()
        {
            var requests = await _accountDal.GetAccountRequests();
            return new SuccessDataResult<List<AccountRequestDto>>(requests, Messages.RetrieveSuccessful);
        }

        public async Task<IDataResult<AccountRequestDto>> GetAccountRequestById(int id)
        {
            var request = await _accountDal.GetAccountRequestById(id);
            return new SuccessDataResult<AccountRequestDto>(request, Messages.RetrieveSuccessful);
        }

        public async Task<IDataResult<RequestCountsDto>> GetRequestCounts()
        {
            var counts = await _accountDal.GetRequestCounts();
            return new SuccessDataResult<RequestCountsDto>(counts, Messages.RetrieveSuccessful);
        }

        public async Task<IResult> Add(Account account)
        {
            await _accountDal.Add(account);
            return new SuccessResult(Messages.AddSuccessful);
        }

        public async Task<IResult> Update(Account account)
        {
            await _accountDal.Update(account);
            return new SuccessResult(Messages.UpdateSuccessful);
        }

        public async Task<IResult> Delete(Account account)
        {
            await _accountDal.Delete(account);
            return new SuccessResult(Messages.DeleteSuccessful);
        }

        public async Task<IResult> AutoCreateAccount(CreateAccountDto accountCreateDto)
        {
            var account = new Account
            {
                UserId = accountCreateDto.UserId,
                AccountNumber = GenerateAccountNumber(),
                AccountType = accountCreateDto.AccountType,
                Balance = 0,
                Status = "Pending",
                CreatedDate = DateTime.Now
            };

            await _accountDal.Add(account);
            return new SuccessResult(Messages.AddSuccessful);
        }

        public async Task<IDataResult<decimal>> TransferMoney(string senderAccountId, string receiverAccountId, decimal amount)
        {
            if (amount <= 0)
                return new ErrorDataResult<decimal>(Messages.InvalidInformation);

            try
            {
                var receiverResult = await GetByAccountNumber(receiverAccountId);
                if (!receiverResult.Success || receiverResult.Data == null)
                    return new ErrorDataResult<decimal>("Receiver account not found.");

                var senderResult = await GetByAccountNumber(senderAccountId);
                bool senderIsAccount = senderResult.Success && senderResult.Data != null;

                if (senderIsAccount)
                {
                    var sender = senderResult.Data;
                    var receiver = receiverResult.Data;

                    if (sender.Id == receiver.Id)
                        return new ErrorDataResult<decimal>(Messages.InvalidInformation);

                    if (sender.Currency != receiver.Currency)
                        return new ErrorDataResult<decimal>("Accounts use different currencies.");

                    if (sender.Balance < amount)
                        return new ErrorDataResult<decimal>(Messages.MoneyTransferFailed);

                    sender.Balance -= amount;
                    receiver.Balance += amount;

                    await _accountDal.Update(sender);
                    await _accountDal.Update(receiver);

                    return new SuccessDataResult<decimal>(sender.Balance, Messages.MoneyTransferSuccessful);
                }
                else
                {
                    var senderCardResult = await _cardService.GetByCardNumber(senderAccountId);
                    if (!senderCardResult.Success || senderCardResult.Data == null)
                        return new ErrorDataResult<decimal>(Messages.InvalidInformation);

                    var senderCard = senderCardResult.Data;
                    var receiver = receiverResult.Data;

                    if (senderCard.Limit < amount)
                        return new ErrorDataResult<decimal>(Messages.MoneyTransferFailed);

                    senderCard.Limit -= amount;
                    receiver.Balance += amount;

                    //await _cardService.Update(senderCard);
                    await _accountDal.Update(receiver);

                    return new SuccessDataResult<decimal>(senderCard.Limit!.Value, Messages.MoneyTransferSuccessful);
                }
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<decimal>($"{Messages.MoneyTransferFailed} Error: {ex.Message}");
            }
        }

        public async Task<IDataResult<decimal>>  WithdrawOrDeposit(DepositWithdrawDto depositWithdrawDto)
        {
            var senderResult = await GetByAccountNumber(depositWithdrawDto.AccountId);

            if (!senderResult.Success || senderResult.Data == null)
                return new ErrorDataResult<decimal>(Messages.InvalidInformation);

            try
            {
                if (depositWithdrawDto.TransactionType == "Withdrawal")
                {
                    if (senderResult.Data.Balance < depositWithdrawDto.Amount)
                        return new ErrorDataResult<decimal>(Messages.MoneyTransferFailed);
                    senderResult.Data.Balance -= depositWithdrawDto.Amount;
                }
                else if (depositWithdrawDto.TransactionType == "Deposit")
                {
                    senderResult.Data.Balance += depositWithdrawDto.Amount;
                }

                await _accountDal.Update(senderResult.Data);

                return new SuccessDataResult<decimal>(senderResult.Data.Balance, Messages.MoneyTransferSuccessful);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<decimal>($"{Messages.MoneyTransferFailed} Error: {ex.Message}");
            }
        }

        private string GenerateAccountNumber()
        {
            var random = new Random();
            return random.Next(10000000, 100000000).ToString();
        }

        public async Task<IDataResult<List<Account>>> GetAll()
        {
            var accounts = await _accountDal.GetAll();
            return new SuccessDataResult<List<Account>>(accounts, Messages.GetAllSuccessful);
        }

        public async Task<IDataResult<List<Account>>> GetAllByUserId(int userId)
        {
            var accounts = await _accountDal.GetAll(a => a.UserId == userId && a.Status != "Pending" && a.Status != "Rejected");
            return new SuccessDataResult<List<Account>>(accounts, Messages.GetByIdSuccessful);
        }

        public async Task<IDataResult<Account>> GetById(int id)
        {
            var account = await _accountDal.Get(a => a.Id == id);
            return new SuccessDataResult<Account>(account, Messages.GetByIdSuccessful);
        }

        public async Task<IDataResult<Account>> GetByAccountNumber(string accountNumber) 
        {
            var account = await _accountDal.Get(a => a.AccountNumber == accountNumber);
            return new SuccessDataResult<Account>(account, Messages.GetByIdSuccessful);
        }

        public async Task<Account> GetUserFirstCheckingAccount(int userId)
        {
            var accounts = await _accountDal.GetAll(a => a.UserId == userId);
            return accounts.FirstOrDefault(a => a.AccountType == "Checking")!;
        }
 
        public async Task<IDataResult<AssetsDto>> GetAssetsAsync(int userId)
        {
            var accounts = await _accountDal.GetAccountsByUserIdAsync(userId);
            var cards = await _cardService.GetCardsByUserIdAsync(userId);

            decimal totalMoney = accounts.Sum(a => a.Balance);
            decimal totalDebt = cards.Data.Sum(c => c.Limit ?? 0);
            int upperLimit = (int)(Math.Ceiling(totalDebt / 5000) * 5000);

            var result = new AssetsDto
            {
                Accounts = accounts,
                Cards = cards.Data,
                TotalBalance = totalMoney,
                TotalDebt = upperLimit - totalDebt
            };
            return new SuccessDataResult<AssetsDto>(result, Messages.GetByIdSuccessful);
        }

        public async Task<List<int>> GetUserAccountIds(int userId)
        {
            return new List<int>(_accountDal.GetUserAccountIds(userId));
        }

        public async Task<IResult> UpdateAccountStatus(UpdateStatusDto updateStatusDto)
        {
            var success = await _accountDal.UpdateAccountStatus(updateStatusDto.Id!.Value, updateStatusDto.Status!);
            if (success)
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
