using Bank.Business.Abstract;
using Bank.Core.Extensions;
using Bank.Core.Utilities.XMLSerializeToXML;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using Bank.XMLWebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Bank.XMLWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/xml")]
    [Produces("application/xml")]
    public class CardTransactionController : BaseController
    {
        private readonly ICardTransactionService _cardTransactionService;

        public CardTransactionController(ICardTransactionService cardTransactionService)
        {
            _cardTransactionService = cardTransactionService;
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll() 
        {
            var result = await _cardTransactionService.GetAll();

            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetByIdAsXml(int id)
        {
            var result = await _cardTransactionService.GetById(id);

            if (!result.Success)
                return BadRequest(result);

            var transaction = result.Data;

            var xml = new XDocument(
                new XElement("CardTransaction",
                    new XElement("id", transaction.Id),
                    new XElement("transactionType", transaction.TransactionType),
                    new XElement("amount", transaction.Amount),
                    new XElement("transactionDate", transaction.TransactionDate.ToString("yyyy-MM-ddTHH:mm:ss")),
                    new XElement("status", transaction.Status)
                )
            );

            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }



        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CardTransaction cardTransaction)
        {
            string xmlString = XmlHelper.SerializeToXml(cardTransaction);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\CardTransaction.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await Task.Run(() => _cardTransactionService.Add(cardTransaction));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] CardTransaction cardTransaction)
        {
            string xmlString = XmlHelper.SerializeToXml(cardTransaction);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\CardTransaction.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await Task.Run(() => _cardTransactionService.Update(cardTransaction));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] CardTransaction cardTransaction)
        {
            string xmlString = XmlHelper.SerializeToXml(cardTransaction);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\CardTransaction.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await Task.Run(() => _cardTransactionService.Delete(cardTransaction));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getlast4transactions")]
        public async Task<IActionResult> GetLast4Transactions()
        {
            int userId = GetUserIdFromToken();
            var result = await Task.Run(() => _cardTransactionService.GetLast4CardTransactionsForUser(userId));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("performtransactionwithcard")]
        public async Task<IActionResult> PerformTransactionWithCard([FromBody] CardTransactionDto cardTransactionDto)
        {
            string xmlString = XmlHelper.SerializeToXml(cardTransactionDto);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\CardTransactionDto.xsd"; 


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            cardTransactionDto.UserId = GetUserIdFromToken();
            var result = await Task.Run(() => _cardTransactionService.PerformTransactionWithCard(cardTransactionDto));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

    }

}
