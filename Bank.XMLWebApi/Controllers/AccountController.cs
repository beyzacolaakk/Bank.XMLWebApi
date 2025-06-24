using Bank.Business.Abstract;
using Bank.Core.Extensions;
using Bank.Core.Utilities.Results;
using Bank.Core.Utilities.XMLSerializeToXML;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.XMLWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet("getall/xml")]
        public async Task<IActionResult> GetAllAsXml()
        {
            var result = await _accountService.GetAll();


            var xmlString = XmlHelper.SerializeToXml(result.Data);

  
            string xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "Account.xsd");


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);

            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }

            return Content(xmlString, "application/xml");
        }


        [HttpGet("getbyid/xml/{id}")]
        public async Task<IActionResult> GetByIdAsXml([FromRoute] int id)
        {
            var result = await _accountService.GetAccountRequestById(id);

            if (!result.Success)
                return BadRequest(result);

            string xmlString = XmlHelper.SerializeToXml(result.Data);

            string xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "Account.xsd");


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);

            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }

            return Content(xmlString, "application/xml");
        }


        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getallbyuserid")]
        public async Task<IActionResult> GetAllByUserId()
        {
            int userId = GetUserIdFromToken();
            var result = await _accountService.GetAllByUserId(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Account account)
        {
            var result = await _accountService.Add(account);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getassets")]
        public async Task<IActionResult> GetAssets()
        {
            int userId = GetUserIdFromToken();
            var result = await _accountService.GetAssetsAsync(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("getaccountrequests")]
        public async Task<IActionResult> GetAccountRequests()
        {
            var result = await _accountService.GetAccountRequests();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("getrequestcounts")]
        public async Task<IActionResult> GetRequestCounts()
        {
            var result = await _accountService.GetRequestCounts();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("createautomaticaccount")]
        public async Task<IActionResult> CreateAutomaticAccount([FromBody] CreateAccountDto createAccountDto)
        {
            createAccountDto.UserId = GetUserIdFromToken();
            var result = await _accountService.AutoCreateAccount(createAccountDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Account account)
        {
            var result = await _accountService.Update(account);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("updateaccountstatus")]
        public async Task<IActionResult> UpdateAccountStatus([FromBody] UpdateStatusDto updateStatusDto)
        {
            var result = await _accountService.UpdateAccountStatus(updateStatusDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] Account account)
        {
            var result = await _accountService.Delete(account);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

    }

}
