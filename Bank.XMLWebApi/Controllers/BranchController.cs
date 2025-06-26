using Bank.Business.Abstract;
using Bank.Core.Extensions;
using Bank.Core.Utilities.Results;
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
    [ApiVersion("2.0")]
    [ApiVersion("1.0")]
    [Consumes("application/xml")]
    [Produces("application/xml")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsXml()
        {
            var result = await _branchService.GetAll();

            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("getbranches")]
        public async Task<IActionResult> GetBranches()
        {
            var result = await _branchService.GetBranches();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [MapToApiVersion("2.0")]
        [HttpGet("getbranches")]
        public async Task<IActionResult> GetBranchesAsXml()
        {
            var result = await _branchService.GetAll(); // Tüm branch'leri al

            if (!result.Success || result.Data == null)
                return BadRequest(result);

            string xml;

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("Branches");

                    foreach (var b in result.Data)
                    {
                        xmlWriter.WriteStartElement("Branch");
                        xmlWriter.WriteElementString("Id", b.Id.ToString());
                        xmlWriter.WriteElementString("BranchName", b.BranchName);
                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteEndElement(); // </Branches>
                    xmlWriter.WriteEndDocument();

                    xmlWriter.Flush(); // 💥 ÖNEMLİ
                }

                xml = stringWriter.ToString(); // Bu artık dolu olacak
            }
            var wrapper = XmlConverter.Deserialize<BranchesWrapper>(xml);
            var  bra= new SuccessDataResult<List<BranchDto>>(wrapper.Items, "Branches retrieved successfully"); 

            if (result.Success)
                return Ok(bra);
            return BadRequest(result);
        }
        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _branchService.GetById(id);
            var res=XmlConverter.Serialize(result.Data);
            if (result.Success)
                return Content(res, "application/xml");
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Branch branch)
        {
            string xmlString = XmlHelper.SerializeToXml(branch);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\Branch.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await _branchService.Add(branch);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Branch branch)
        {
            string xmlString = XmlHelper.SerializeToXml(branch);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\Branch.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await _branchService.Update(branch);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] Branch branch)
        {
            string xmlString = XmlHelper.SerializeToXml(branch);

            string xsdPath = @"C:\Users\fb_go\source\repos\Bank.XMLWebApi\Bank.XMLWebApi\Schemas\Branch.xsd";


            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);
            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            var result = await _branchService.Delete(branch);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }

}
