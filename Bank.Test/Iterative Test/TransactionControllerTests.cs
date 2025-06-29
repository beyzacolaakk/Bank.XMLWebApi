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
        [Theory]
        [InlineData("account", true)]
        [InlineData("card", true)]
        [InlineData("invalid", false)]
        public async Task SendMoney_ShouldBehaveCorrectly_BasedOnPaymentMethod(string paymentMethod, bool expectedSuccess)
        {
            var dto = new MoneyTransferDto
            {
                UserId = 5,
                SenderAccountId = paymentMethod == "card" ? "4111111111111111" : "ACC123",
                ReceiverAccountId = "ACC456",
                Amount = 1500.00m,
                TransactionType = "Transfer",
                Description = "Salary",
                PaymentMethod = paymentMethod
            };

            if (expectedSuccess)
            {
                _transactionServiceMock.Setup(s => s.SendMoney(dto))
                    .ReturnsAsync(new SuccessResult("Money transferred."));
            }
            else
            {
                _transactionServiceMock.Setup(s => s.SendMoney(dto))
                    .ReturnsAsync(new ErrorResult("Invalid payment method."));
            }

            var result = await _controller.SendMoney(dto);

            if (expectedSuccess)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var response = Assert.IsAssignableFrom<IResult>(okResult.Value);
                Assert.True(response.Success);
                Assert.Equal("Money transferred.", response.Message);
            }
            else
            {
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                var response = Assert.IsAssignableFrom<IResult>(badRequest.Value);
                Assert.False(response.Success);
            }
        }



    }
}
