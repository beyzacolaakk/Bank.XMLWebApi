using Bank.Business.Abstract;
using Bank.Core.Extensions;
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
    public class SupportRequestController : BaseController
    {
        private readonly ISupportRequestService _supportRequestService;

        public SupportRequestController(ISupportRequestService supportRequestService)
        {
            _supportRequestService = supportRequestService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _supportRequestService.GetAll();

            if (!result.Success)
            {
                return BadRequest(result);
            }

            var xml = XmlConverter.Serialize(result.Data);

            string xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "SupportRequest.xsd");

            bool isValid = XmlValidator.ValidateXml(xml, xsdPath, out var errors);

            if (!isValid)
            {
                return BadRequest(new { Message = "XML validation failed", Errors = errors });
            }

            return Content(xml, "application/xml");
        }


        [HttpGet("getallbyuserid")]
        public async Task<IActionResult> GetAllByUserId()
        {
            int userId = GetUserIdFromToken();
            var result = await _supportRequestService.GetAllByUserId(userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            var xml = XmlConverter.Serialize(result.Data);

            string xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "SupportRequest.xsd");

            bool isValid = XmlValidator.ValidateXml(xml, xsdPath, out var errors);

            if (!isValid)
            {
                return BadRequest(new { Message = "XML validation failed", Errors = errors });
            }

            return Content(xml, "application/xml");
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _supportRequestService.GetById(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getsupportrequests")]
        public async Task<IActionResult> GetSupportRequests()
        {
            var result = await _supportRequestService.GetAll();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] SupportRequest supportRequest)
        {
            var result = await _supportRequestService.Add(supportRequest);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] SupportRequest supportRequest)
        {
            var result = await _supportRequestService.Update(supportRequest);
            return result.Success ? Ok(result) : BadRequest(result);
        }



        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("createsupportrequest")]
        public async Task<IActionResult> CreateSupportRequest([FromBody] SupportRequestDto createDto)
        {
            createDto.UserId = GetUserIdFromToken();
            var result = await _supportRequestService.CreateSupportRequest(createDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _supportRequestService.Delete(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string status = "all", [FromQuery] string search = "")
        {
            var userId = GetUserIdFromToken();
            var result = await _supportRequestService.FilterSupportRequests(userId, "all", ""); // önce tüm veriyi çek

            if (!result.Success)
                return BadRequest(result.Message);

         
            string xmlString = XmlConverter.Serialize(result.Data);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);

          
            var conditions = new List<string>();

            if (!string.Equals(status, "all", StringComparison.OrdinalIgnoreCase))
            {
               
                conditions.Add($"translate(Status, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = '{status.ToLower()}'");
            }

            if (!string.IsNullOrEmpty(search))
            {
                var loweredSearch = search.ToLower();
               
                conditions.Add($"contains(translate(Subject, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{loweredSearch}') or contains(translate(Message, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{loweredSearch}')");
            }

            string xpath = "//SupportRequest";
            if (conditions.Count > 0)
            {
                xpath += "[" + string.Join(" and ", conditions) + "]";
            }

            XmlNodeList filteredNodes = xmlDoc.SelectNodes(xpath, nsmgr)!;

            if (filteredNodes == null || filteredNodes.Count == 0)
                return NotFound("No matching support requests found.");

            XmlDocument filteredXml = new XmlDocument();
            XmlElement root = filteredXml.CreateElement("SupportRequests");
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
