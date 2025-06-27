using Bank.Business.Abstract;
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
    public class CardControllerTests
    {
        private readonly CardController _controller;
        private readonly Mock<ICardService> _cardServiceMock;

        public CardControllerTests()
        {
            _cardServiceMock = new Mock<ICardService>();
            _controller = new CardController(_cardServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Theory]
        [InlineData("Credit")]
        [InlineData("Bank")]
        public async Task CreateAutomaticCard_ShouldReturnOk_AndGenerateValidXml(string cardType)
        {
   
            var dto = new CreateCardDto
            {
                UserId = 5,
                CardType = cardType
            };

     
            var xml = XmlTestHelper.SerializeToXml(dto);
            Assert.Contains("CreateCardDto", xml); // kök etiketi içeriyor mu?

  
            Assert.False(string.IsNullOrWhiteSpace(xml), "Generated XML is empty");

            _cardServiceMock
                .Setup(x => x.AutoCreateCard(It.IsAny<CreateCardDto>()))
                .ReturnsAsync(new SuccessResult("Card created automatically."));

      
            var result = await _controller.CreateAutomaticCard(dto);

    
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<IResult>(okResult.Value);
            Assert.True(value.Success);
        }
        [Fact]
        public async Task GetCardRequests_ShouldReturnOk_WithValidData()
        {
    
            var fakeList = new List<CardRequestDto>
    {
        new CardRequestDto
        {
            Id = 1,
            FullName = "Gökdeniz Genç",
            CardType = "Credit Card",
            Limit = 5000,
            Date = DateTime.Now,
            Status = "Pending"
        }
    };

            _cardServiceMock.Setup(x => x.GetCardRequests())
                .ReturnsAsync(new SuccessDataResult<List<CardRequestDto>>(fakeList));

    
            var result = await _controller.GetCardRequests();

    
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<IDataResult<List<CardRequestDto>>>(okResult.Value);
            Assert.True(data.Success);
            Assert.Single(data.Data);
        }
        [Fact]
        public async Task GetCardRequests_ShouldReturnEmptyList_IfNoRequestsExist()
        {
  
            var emptyList = new List<CardRequestDto>();
            _cardServiceMock.Setup(x => x.GetCardRequests())
                .ReturnsAsync(new SuccessDataResult<List<CardRequestDto>>(emptyList));

   
            var result = await _controller.GetCardRequests();


            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<IDataResult<List<CardRequestDto>>>(okResult.Value);
            Assert.True(data.Success);
            Assert.Empty(data.Data);
        }
        [Fact]
        public async Task GetCardRequests_ShouldReturnBadRequest_IfServiceFails()
        {

            _cardServiceMock.Setup(x => x.GetCardRequests())
                .ReturnsAsync(new ErrorDataResult<List<CardRequestDto>>("Service error"));


            var result = await _controller.GetCardRequests();


            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var data = Assert.IsAssignableFrom<ErrorDataResult<List<CardRequestDto>>>(badRequest.Value);
            Assert.False(data.Success);
            Assert.Equal("Service error", data.Message);
        }
        [Fact]
        public void CardRequestDto_Serialization_ShouldProduceValidXml()
        {
            var dto = new CardRequestDto
            {
                Id =11,
                FullName = "Test User",
                CardType = "Credit",
                Limit = 1000,
                Date = DateTime.Now,
                Status = "Active"
            };

            string xml = XmlHelper.SerializeToXml(dto);

            Assert.StartsWith("<?xml", xml);
            Assert.Contains("<CardType>Credit</CardType>", xml);
        }



    }

}
