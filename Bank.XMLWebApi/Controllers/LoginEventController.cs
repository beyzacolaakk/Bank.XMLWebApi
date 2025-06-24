using Bank.Business.Abstract;
using Bank.Core.Extensions;
using Bank.Core.Utilities.XMLSerializeToXML;
using Bank.Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.XMLWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginEventController : ControllerBase
    {
        private readonly ILoginEventService _loginEventService;

        public LoginEventController(ILoginEventService loginEventService)
        {
            _loginEventService = loginEventService;
        }

        [HttpGet("getall/xml")]
        public async Task<IActionResult> GetAllAsXml()
        {
            var result = await _loginEventService.GetAll("Time", true);

            if (!result.Success)
                return BadRequest(result);

            string xmlString = XmlHelper.SerializeToXml(result.Data);


            string xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "LoginEvent.xsd");


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


        [Authorize(Roles = "Administrator")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _loginEventService.GetById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] LoginEvent loginEvent)
        {
            var result = await _loginEventService.Add(loginEvent);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] LoginEvent loginEvent)
        {
            var result = await _loginEventService.Update(loginEvent);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] LoginEvent loginEvent)
        {
            var result = await _loginEventService.Delete(loginEvent);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }

}
