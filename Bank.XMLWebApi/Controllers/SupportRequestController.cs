using Bank.Business.Abstract;
using Bank.Business.Concrete;
using Bank.Core.Entities.Concrete;
using Bank.Core.Extensions;
using Bank.Core.Utilities.XMLSerializeToXML;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
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
    public class SupportRequestController : BaseController
    {
        private readonly ISupportRequestService _supportRequestService;

        public SupportRequestController(ISupportRequestService supportRequestService) 
        {
            _supportRequestService = supportRequestService;

        }
        [Authorize(Roles = "Administrator")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _supportRequestService.GetAll();

            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getallbyuserid")]
        public async Task<IActionResult> GetAllByUserId()
        {
            int userId = GetUserIdFromToken();
            var result = await _supportRequestService.GetAllByUserId(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _supportRequestService.GetById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getsupportrequests")]
        public async Task<IActionResult> GetSupportRequests()
        {
            var result = await _supportRequestService.GetSupportRequests();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] SupportRequest supportRequest)
        {
            string xmlString = XmlHelper.SerializeToXml(supportRequest);

            string xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "SupportRequest.xsd");

            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await _supportRequestService.Add(supportRequest);

            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] SupportRequest supportRequest)
        {
            string xmlString = XmlHelper.SerializeToXml(supportRequest);

            string xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "SupportRequest.xsd");

            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await _supportRequestService.Update(supportRequest);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPut("updatesupportrequest")]
        public async Task<IActionResult> DestekTalebiDurumGuncelle([FromBody] SupportRequestUpdateDto destekTalebiGuncelle)
        {
            var sonuc = await _supportRequestService.UpdateSupportRequestStatus(destekTalebiGuncelle);
            if (sonuc.Success)
                return Ok(sonuc);
            return BadRequest(sonuc);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("createsupportrequest")]
        public async Task<IActionResult> CreateSupportRequest([FromBody] SupportRequestDto supportRequestDto)
        {
            supportRequestDto.UserId = GetUserIdFromToken();
            var result = await _supportRequestService.CreateSupportRequest(supportRequestDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var result = await _supportRequestService.Delete(id);
            var resultxml = XmlConverter.Serialize(result.Message);
            if (!result.Success)
            {
                return BadRequest(resultxml);
            }
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string status = "all", [FromQuery] string search = "")
        {
            var userId = GetUserIdFromToken();
            var result = await _supportRequestService.FilterSupportRequests(userId, status, "");

            if (!result.Success)
                return BadRequest(result.Message);

            string xmlString = XmlConverter.Serialize(result.Data);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);

            var conditions = new List<string>();

            if (!string.Equals(status, "all", StringComparison.OrdinalIgnoreCase))
            {
                var escapedStatus = System.Security.SecurityElement.Escape(status.ToLower());
                conditions.Add($"translate(status, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = '{escapedStatus}'");
            }

            if (!string.IsNullOrEmpty(search))
            {
                var escapedSearch = System.Security.SecurityElement.Escape(search.ToLower());
                conditions.Add($"contains(translate(subject, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{escapedSearch}') or contains(translate(message, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{escapedSearch}')");
            }

            string xpath = "//SupportRequest";
            if (conditions.Count > 0)
            {
                xpath += "[" + string.Join(" and ", conditions) + "]";
            }

            XmlNodeList filteredNodes = xmlDoc.SelectNodes(xpath, nsmgr);

            if (filteredNodes == null || filteredNodes.Count == 0)
                return NotFound("No matching support requests found.");

            XmlDocument filteredXml = new XmlDocument();
            XmlElement root = filteredXml.CreateElement("SupportRequestList");
            filteredXml.AppendChild(root);

            foreach (XmlNode node in filteredNodes)
            {
                XmlNode imported = filteredXml.ImportNode(node, true);
                root.AppendChild(imported);
            }
            
            return Content(filteredXml.OuterXml, "application/xml");
        }






    }
}
