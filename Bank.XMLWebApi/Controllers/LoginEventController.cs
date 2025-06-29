using Bank.Business.Abstract;
using Bank.Core.Extensions;
using Bank.Core.Utilities.XMLSerializeToXML;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Xsl;
using System.Xml;

namespace Bank.XMLWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiVersion("1.0")]
    [Consumes("application/xml")]
    [Produces("application/xml")]
    public class LoginEventController : ControllerBase
    {
        private readonly ILoginEventService _loginEventService;

        public LoginEventController(ILoginEventService loginEventService)
        {
            _loginEventService = loginEventService;
        }
        [Authorize(Roles = "Administrator")]
        [MapToApiVersion("1.0")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsXml()
        {
            var result = await _loginEventService.GetAll("Time", true);

            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [Authorize(Roles = "Administrator")]
        [MapToApiVersion("2.0")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetLoginEventsHtml()
        {
            var result = await _loginEventService.GetAll("Time", true);

            if (!result.Success)
                return BadRequest(result.Message);

   
            string xmlString = XmlConverter.Serialize(result.Data);


            var xslt = new XslCompiledTransform();
            xslt.Load("Schemas/loginEvents.xslt");

            var xmlReader = XmlReader.Create(new StringReader(xmlString));
            var stringWriter = new StringWriter();
            xslt.Transform(xmlReader, null, stringWriter);

            return Content(stringWriter.ToString(), "text/html");
        }


        [Authorize(Roles = "Administrator")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _loginEventService.GetById(id);
            string xmlString = XmlHelper.SerializeToXml(result.Data);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] LoginEvent loginEvent)
        {
            string xmlString = XmlHelper.SerializeToXml(loginEvent);

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
            var result = await _loginEventService.Add(loginEvent);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] LoginEvent loginEvent)
        {
            string xmlString = XmlHelper.SerializeToXml(loginEvent);

 
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
            var result = await _loginEventService.Update(loginEvent);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] LoginEvent loginEvent)
        {
            string xmlString = XmlHelper.SerializeToXml(loginEvent);

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
            var result = await _loginEventService.Delete(loginEvent);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }

}
