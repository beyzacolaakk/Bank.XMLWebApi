using Bank.Business.Abstract;
using Bank.Core.Utilities.Results;
using Bank.Core.Utilities.XMLSerializeToXML;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using Bank.XMLWebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IResult = Bank.Core.Utilities.Results.IResult;

namespace Bank.Test.Iterative_Test
{
    public class TransactionControllerTests
    {
        private readonly TransactionController _controller;
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly Mock<IAccountService> _accountServiceMock;

        public TransactionControllerTests() 
        {
            _transactionServiceMock = new Mock<ITransactionService>();
            _accountServiceMock = new Mock<IAccountService>();
            _controller = new TransactionController(_transactionServiceMock.Object) 
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }
        [Fact]
        public async Task SendMoney_ShouldReturnOk_WhenAccountTransferIsSuccessful()
        {
            var dto = new MoneyTransferDto
            {
                UserId = 5,
                SenderAccountId = "ACC123",
                ReceiverAccountId = "ACC456",
                Amount = 1500.00m,
                TransactionType = "Transfer",
                Description = "Salary",
                PaymentMethod = "account"
            };

            _transactionServiceMock.Setup(s => s.SendMoney(It.IsAny<MoneyTransferDto>()))
                .ReturnsAsync(new SuccessResult("Money transferred."));

            var result = await _controller.SendMoney(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<IResult>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Money transferred.", response.Message);
        }
        [Fact]
        public async Task SendMoney_ShouldReturnError_WhenReceiverNotFound()
        {
            var dto = GetValidDto();

            _accountServiceMock.Setup(x => x.GetByAccountNumber(dto.ReceiverAccountId))
                .ReturnsAsync(new ErrorDataResult<Account>("Recipient account not found."));

            _transactionServiceMock.Setup(x => x.SendMoney(dto))
                .ReturnsAsync(new ErrorResult("Recipient account not found."));

            var result = await _controller.SendMoney(dto);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsAssignableFrom<IResult>(badRequest.Value);
            Assert.False(response.Success);
        }
        [Fact]
        public async Task SendMoney_ShouldWork_WhenPaymentMethodIsCard()
        {
            var dto = GetValidDto();
            dto.PaymentMethod = "card";
            dto.SenderAccountId = "4111111111111111"; 

            _transactionServiceMock.Setup(x => x.SendMoney(dto))
                .ReturnsAsync(new SuccessResult("Transfer successful"));

            var result = await _controller.SendMoney(dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<IResult>(ok.Value);
            Assert.True(value.Success);
        }
        private MoneyTransferDto GetValidDto() => new MoneyTransferDto
        {
            UserId = 5,
            SenderAccountId = "ACC123",
            ReceiverAccountId = "ACC456",
            Amount = 100,
            TransactionType = "Transfer",
            Description = "Test",
            PaymentMethod = "account"
        };


    }
}
