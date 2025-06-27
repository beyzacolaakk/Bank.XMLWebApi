using Bank.Business.Abstract;
using Bank.Core.Extensions;
using Bank.Core.Utilities.Results;
using Bank.Core.Utilities.XMLSerializeToXML;
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
    public class LimitIncreaseControllerTests
    {
        private readonly Mock<ILimitIncreaseService> _mockService;
        private readonly LimitIncreaseController _controller;

        public LimitIncreaseControllerTests()
        {
            _mockService = new Mock<ILimitIncreaseService>();
            _controller = new LimitIncreaseController(_mockService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Theory]
        [InlineData(1, 1000.00, 2000.00, true)]     
        [InlineData(0, 1000.00, 2000.00, false)]    
        [InlineData(1, -100.00, 2000.00, false)]    
        [InlineData(1, 1000.00, 500.00, true)]      
        [InlineData(1, 1000.00, 0.00, false)]      

        public async Task AddLimitIncrease_ShouldValidateXmlAndReturnResult(int cardId, decimal currentLimit, decimal newLimit, bool expectedSuccess)
        {
            var dto = new LimitIncreaseRequestDto
            {
                CardId = cardId,
                CurrentLimit = currentLimit,
                NewLimit = newLimit
            };

            string xml = XmlHelper.SerializeToXml(dto);
            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\LimitIncreaseRequestDto.xsd";

            bool isValid = XmlValidator.ValidateXml(xml, xsdPath, out var errors);

            if (!isValid)
            {
                Assert.False(expectedSuccess);
                Assert.False(string.IsNullOrEmpty(errors[0]));
                return;
            }

            _mockService.Setup(s => s.AddLimitIncreaseRequest(It.IsAny<LimitIncreaseRequestDto>()))
                .ReturnsAsync(new SuccessResult("Limit increase added."));

   
            var result = await _controller.AddLimitIncrease(dto);

     
            if (expectedSuccess)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var value = Assert.IsAssignableFrom<IResult>(okResult.Value);
                Assert.True(value.Success);
            }
            else
            {
                var badResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.NotNull(badResult.Value);
            }
        }
    }
}
