using Bank.Business.Abstract;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using Bank.Core.Extensions;
using Bank.Core.Utilities.XMLSerializeToXML;
using Bank.Core.Entities.Concrete;

namespace Bank.XMLWebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/xml")]
    [Consumes("application/xml")]
    [ApiController]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _transactionService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _transactionService.GetById(id);
            string xmlString = XmlHelper.SerializeToXml(result.Data);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getlast4")]
        public async Task<IActionResult> GetLast4Transactions()
        {
            var userId = GetUserIdFromToken();
            var result = await _transactionService.GetLast4CardTransactionsForUser(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("sendmoney")]
        public async Task<IActionResult> SendMoney([FromBody] MoneyTransferDto sendMoneyDto)
        {
            string xmlString = XmlHelper.SerializeToXml(sendMoneyDto);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\MoneyTransferDto.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            sendMoneyDto.UserId = GetUserIdFromToken();
            var result = await Task.Run(() => _transactionService.SendMoney(sendMoneyDto));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Transaction transaction)
        {
            // XML serialize et, sonra varsa XML declaration'ı temizle
            string xmlString = XmlHelper.SerializeToXml(transaction);

            if (xmlString.StartsWith("<?xml"))
            {
                int index = xmlString.IndexOf("?>");
                if (index >= 0)
                {
                    xmlString = xmlString.Substring(index + 2).TrimStart();
                }
            }

            string dtdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\Transaction.DTD";

            try
            {
                bool isValid = XmlValidator.ValidateXmlWithDtd(xmlString, dtdPath, out var errors);
                if (!isValid)
                {
                    return BadRequest(new
                    {
                        Message = "XML validation failed",
                        Errors = errors
                    });
                }
                var result = await Task.Run(() => _transactionService.Add(transaction));

                if (result.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (XmlException ex)
            {
                return BadRequest($"XML validation failed: {ex.Message}");
            }
        }


        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("depositwithdraw")]
        public async Task<IActionResult> DepositWithdraw([FromBody] DepositWithdrawDto depositWithdrawDto)
        {
            string xmlString = XmlHelper.SerializeToXml(depositWithdrawDto);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\DepositWithdrawDto.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            depositWithdrawDto.UserId = GetUserIdFromToken();
            var result = await Task.Run(() => _transactionService.WithdrawOrDeposit(depositWithdrawDto));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Transaction transaction)
        {
            string xmlString = XmlHelper.SerializeToXml(transaction);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\Transaction.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await Task.Run(() => _transactionService.Update(transaction));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] Transaction transaction)
        {
            string xmlString = XmlHelper.SerializeToXml(transaction);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\Transaction.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await Task.Run(() => _transactionService.Delete(transaction));
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

    }

}
