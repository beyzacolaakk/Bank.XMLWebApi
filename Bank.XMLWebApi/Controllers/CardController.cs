

using Bank.Business.Abstract;
using Bank.Core.Extensions;
using Bank.Core.Utilities.Results;
using Bank.Core.Utilities.XMLSerializeToXML;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using Bank.XMLWebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace Bank.XMLWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/xml")]
    [Produces("application/xml")]
    public class CardController : BaseController
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsXml()
        {
            var result = await _cardService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetCardPartialXml([FromRoute] int id)
        {
            var result = await _cardService.GetById(id);

            if (!result.Success || result.Data == null)
                return BadRequest(result);

            string fullXml = XmlHelper.SerializeToXml(result.Data);

            int idVal = 0;
            string cardType = "";
            decimal? limit = null;
            string status = "";
            DateTime expirationDate = DateTime.MinValue;

            using (var reader = XmlReader.Create(new StringReader(fullXml)))
            {
                while (reader.Read())
                {
      
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "id":
                                idVal = reader.ReadElementContentAsInt();
                                break;
                            case "cardType":
                                cardType = reader.ReadElementContentAsString();
                                break;
                 
                            case "limit":
                                if (reader.IsEmptyElement)
                                {
                                    limit = null;
                                    reader.Read(); 
                                }
                                else
                                {
                                    limit = reader.ReadElementContentAsDecimal();
                                }
                                break;
                            case "status":
                                status = reader.ReadElementContentAsString();
                                break;
                            case "expirationDate":
                                expirationDate = reader.ReadElementContentAsDateTime();
                                break;
                        }
                    }
                }
            }

            string partialXml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<CardRequestDto>
    <Id>{idVal}</Id>
    <CardType>{System.Security.SecurityElement.Escape(cardType)}</CardType>
    <Limit>{(limit.HasValue ? limit.Value.ToString() : "")}</Limit>
    <Status>{System.Security.SecurityElement.Escape(status)}</Status>
    <ExpirationDate>{expirationDate.ToString("o")}</ExpirationDate>
</CardRequestDto>";
            var cardreq=XmlConverter.Deserialize<CardRequestDto>(partialXml);
            var res = new SuccessDataResult<CardRequestDto>(cardreq, result.Message);
            if (result.Success)
                return Ok(res);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("createautomaticcard")]
        public async Task<IActionResult> CreateAutomaticCard([FromBody] CreateCardDto createCardDto)
        {
            createCardDto.UserId = GetUserIdFromToken();
            string xmlString = XmlHelper.SerializeToXml(createCardDto);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\CreateCardDto.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }

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

            string xmlString = XmlHelper.SerializeToXml(card);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\Card.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }

            var result = await Task.Run(() => _cardService.Add(card));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Card card)
        {
            string xmlString = XmlHelper.SerializeToXml(card);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\Card.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await Task.Run(() => _cardService.Update(card));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] Card card)
        {
            string xmlString = XmlHelper.SerializeToXml(card);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\Card.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await Task.Run(() => _cardService.Delete(card));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("updatecardstatus")]
        public async Task<IActionResult> UpdateCardStatus([FromBody] UpdateStatusDto updateStatusDto)
        {
            string xmlString = XmlHelper.SerializeToXml(updateStatusDto);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\UpdateStatusDto.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await _cardService.UpdateCardStatus(updateStatusDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }


    }

}
