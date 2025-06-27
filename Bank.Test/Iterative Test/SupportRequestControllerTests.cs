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
    public class SupportRequestControllerTests
    {
        private readonly Mock<ISupportRequestService> _mockService;
        private readonly SupportRequestController _controller;

        public SupportRequestControllerTests()
        {
            _mockService = new Mock<ISupportRequestService>();
            _controller = new SupportRequestController(_mockService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Theory]
        [InlineData("Login Issue", "Having trouble accessing account", "Open", "Security", "John Doe", "2024-06-25T14:30:00", true)]
        [InlineData("Bug54", "App crash helphelphelp", "Process", "Mobile Application", "Jane", "2024-06-20T09:00:00", true)]
        [InlineData("Shrt545", "Too short help", "Open", "Other", "", "2024-06-20T09:00:00", false)] // Invalid subject & message
        [InlineData("Valid Subject", "Valid message helphelphelp ", "InvalidStatus", "InvalidCategory", "User X", "2024-06-20T09:00:00", false)]
        public async Task CreateSupportRequest_IterativeValidation(string subject, string message, string status, string category, string fullName, string dateStr, bool expectedSuccess)
        {
       
            var dto = new SupportRequestDto
            {
                Id = 1,
                UserId = 6,
                Subject = subject,
                Message = message,
                Status = status,
                Response = null,
                Category = category,
                FullName = fullName,
                Date = DateTime.TryParse(dateStr, out var d) ? d : null
            };

            string xml = XmlHelper.SerializeToXml(dto);
            string xsdPath = @"C:\\Users\\fb_go\\source\\repos\\Bank.XMLWebApi\\Bank.XMLWebApi\\Schemas\\SupportRequestDto.xsd";

            bool isValid = XmlValidator.ValidateXml(xml, xsdPath, out var errors);

            if (!isValid)
            {
                Assert.False(expectedSuccess);
                Assert.False(string.IsNullOrWhiteSpace(errors[0]));
                return;
            }

            _mockService.Setup(x => x.CreateSupportRequest(It.IsAny<SupportRequestDto>()))
                .ReturnsAsync(new SuccessResult("Support request created."));

      
            var result = await _controller.CreateSupportRequest(dto);

  
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
