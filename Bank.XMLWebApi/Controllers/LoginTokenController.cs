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
    [Consumes("application/xml")]
    [Produces("application/xml")]
    public class LoginTokenController : ControllerBase
    {
        private readonly ILoginTokenService _loginTokenService;

        public LoginTokenController(ILoginTokenService loginTokenService)
        {
            _loginTokenService = loginTokenService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _loginTokenService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _loginTokenService.GetById(id);
            string xmlString = XmlHelper.SerializeToXml(result.Data);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] LoginToken token)
        {
            string xmlString = XmlHelper.SerializeToXml(token);

            string xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "LoginToken.xsd");

            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await _loginTokenService.Add(token);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] LoginToken token)
        {
            string xmlString = XmlHelper.SerializeToXml(token);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\LoginToken.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await _loginTokenService.Update(token);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] LoginToken token)
        {
            string xmlString = XmlHelper.SerializeToXml(token);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\LoginToken.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await _loginTokenService.Delete(token);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }

}
