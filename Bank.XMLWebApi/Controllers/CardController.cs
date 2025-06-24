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
    public class CardController : BaseController
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet("getall/xml")]
        public async Task<IActionResult> GetAllAsXml()
        {
            var result = await _cardService.GetAll();

            if (!result.Success)
                return BadRequest(result);


            string xmlString = XmlHelper.SerializeToXml(result.Data);


            string xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "Card.xsd");


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);

            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }

            // Başarılıysa XML içerik döndür
            return Content(xmlString, "application/xml");
        }


        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await Task.Run(() => _cardService.GetCardRequestById(id));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("createautomaticcard")]
        public async Task<IActionResult> CreateAutomaticCard([FromBody] CreateCardDto createCardDto)
        {
            createCardDto.UserId = GetUserIdFromToken();
            var result = await _cardService.AutoCreateCard(createCardDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getallbyuserid")]
        public async Task<IActionResult> GetAllByUserId()
        {
            int userId = GetUserIdFromToken();
            var result = await _cardService.GetAllById(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("getcardrequests")]
        public async Task<IActionResult> GetCardRequests()
        {
            var result = await _cardService.GetCardRequests();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Card card)
        {
            var result = await Task.Run(() => _cardService.Add(card));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Card card)
        {
            var result = await Task.Run(() => _cardService.Update(card));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] Card card)
        {
            var result = await Task.Run(() => _cardService.Delete(card));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("updatecardstatus")]
        public async Task<IActionResult> UpdateCardStatus([FromBody] UpdateStatusDto updateStatusDto)
        {
            var result = await _cardService.UpdateCardStatus(updateStatusDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }


    }

}
