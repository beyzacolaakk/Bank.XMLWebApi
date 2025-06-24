using Bank.Business.Abstract;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.XMLWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _transactionService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _transactionService.GetById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getlast4")]
        public async Task<IActionResult> GetLast4Transactions()
        {
            var userId = GetUserIdFromToken();
            var result = await _transactionService.GetLast4CardTransactionsForUser(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("sendmoney")]
        public async Task<IActionResult> SendMoney([FromBody] MoneyTransferDto sendMoneyDto)
        {
            sendMoneyDto.UserId = GetUserIdFromToken();
            var result = await Task.Run(() => _transactionService.SendMoney(sendMoneyDto));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Transaction transaction)
        {
            var result = await Task.Run(() => _transactionService.Add(transaction));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("depositwithdraw")]
        public async Task<IActionResult> DepositWithdraw([FromBody] DepositWithdrawDto depositWithdrawDto)
        {
            depositWithdrawDto.UserId = GetUserIdFromToken();
            var result = await Task.Run(() => _transactionService.WithdrawOrDeposit(depositWithdrawDto));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Transaction transaction)
        {
            var result = await Task.Run(() => _transactionService.Update(transaction));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] Transaction transaction)
        {
            var result = await Task.Run(() => _transactionService.Delete(transaction));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

    }

}
