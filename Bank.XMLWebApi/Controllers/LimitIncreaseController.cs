using Bank.Business.Abstract;
using Bank.Core.Extensions;
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
    [Consumes("application/xml")]
    [Produces("application/xml")]
    public class LimitIncreaseController : ControllerBase
    {
        private ILimitIncreaseService _limitIncreaseService;

        public LimitIncreaseController(ILimitIncreaseService limitIncreaseService)
        {
            _limitIncreaseService = limitIncreaseService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("getcardlimitrequests")]
        public async Task<IActionResult> GetCardLimitRequests()
        {
            var result = await _limitIncreaseService.GetCardLimitRequests();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("addlimitincrease")]
        public async Task<IActionResult> AddLimitIncrease([FromBody] LimitIncreaseRequestDto limitIncrease)
        {
            string xmlString = XmlHelper.SerializeToXml(limitIncrease);
            string xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "LimitIncreaseRequestDto.xsd");


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await _limitIncreaseService.AddLimitIncreaseRequest(limitIncrease);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("updatelimit")]
        public async Task<IActionResult> UpdateLimit([FromBody] LimitIncreaseCreateDto limitIncrease)
        {
            string xmlString = XmlHelper.SerializeToXml(limitIncrease);

            string xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "LimitIncreaseCreateDto.xsd");



            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await _limitIncreaseService.UpdateCardLimitRequest(limitIncrease);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _limitIncreaseService.Delete(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _limitIncreaseService.GetCardLimitRequestById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }

}
