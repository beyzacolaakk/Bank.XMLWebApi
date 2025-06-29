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
        private readonly Mock<IUserService> _userServiceMock;
        public CardControllerTests()
        {
            _cardServiceMock = new Mock<ICardService>();
            _userServiceMock= new Mock<IUserService>();
            _controller = new CardController(_cardServiceMock.Object,_userServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }
        public class CardRequestTestCase
        {
            public List<CardRequestDto> MockData { get; set; }
            public bool ExpectSuccess { get; set; }
            public bool ExpectEmpty { get; set; }
            public string ErrorMessage { get; set; }
        }
        [Fact]
        public async Task GetCardRequests_IterativeTests()
        {
            var testCases = new List<CardRequestTestCase>
    {
        new CardRequestTestCase
        {
            MockData = new List<CardRequestDto>
            {
                new CardRequestDto
                {
                    Id = 1,
                    FullName = "Ahmet KAYA",
                    CardType = "Credit Card",
                    Limit = 5000,
                    Date = DateTime.Now,
                    Status = "Pending"
                }
            },
            ExpectSuccess = true,
            ExpectEmpty = false,
            ErrorMessage = null
        },
        new CardRequestTestCase
        {
            MockData = new List<CardRequestDto>(),
            ExpectSuccess = true,
            ExpectEmpty = true,
            ErrorMessage = null
        },
        new CardRequestTestCase
        {
            MockData = null,
            ExpectSuccess = false,
            ExpectEmpty = true,
            ErrorMessage = "Service error"
        }
    };

            foreach (var testCase in testCases)
            {
                if (testCase.ExpectSuccess)
                {
                    _cardServiceMock.Setup(x => x.GetCardRequests())
                        .ReturnsAsync(new SuccessDataResult<List<CardRequestDto>>(testCase.MockData));
                }
                else
                {
                    _cardServiceMock.Setup(x => x.GetCardRequests())
                        .ReturnsAsync(new ErrorDataResult<List<CardRequestDto>>(testCase.ErrorMessage));
                }

                var result = await _controller.GetCardRequests();

                if (testCase.ExpectSuccess)
                {
                    var okResult = Assert.IsType<OkObjectResult>(result);
                    var data = Assert.IsAssignableFrom<IDataResult<List<CardRequestDto>>>(okResult.Value);
                    Assert.True(data.Success);

                    if (testCase.ExpectEmpty)
                        Assert.Empty(data.Data);
                    else
                        Assert.NotEmpty(data.Data);
                }
                else
                {
                    var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                    var data = Assert.IsAssignableFrom<ErrorDataResult<List<CardRequestDto>>>(badRequest.Value);
                    Assert.False(data.Success);
                    Assert.Equal(testCase.ErrorMessage, data.Message);
                }
            }
        }



    }

}
