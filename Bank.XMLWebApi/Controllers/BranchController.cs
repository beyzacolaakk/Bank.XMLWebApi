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
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet("getall/xml")]
        public async Task<IActionResult> GetAllAsXml()
        {
            var result = await _branchService.GetAll();

            if (!result.Success)
                return BadRequest(result);

            string xmlString = XmlHelper.SerializeToXml(result.Data);
       
            string xsdPath = Path.Combine(Directory.GetCurrentDirectory(), "Schemas", "Branch.xsd");

            bool isValid = XmlValidator.ValidateXml(xmlString, xsdPath, out var errors);

            if (!isValid)
            {
                return BadRequest(new
                {
                    Message = "XML validation failed",
                    Errors = errors
                });
            }
            return Content(xmlString, "application/xml");
        }


        [HttpGet("getbranches")]
        public async Task<IActionResult> GetBranches()
        {
            var result = await _branchService.GetBranches();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _branchService.GetById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] Branch branch)
        {
            var result = await _branchService.Add(branch);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Branch branch)
        {
            var result = await _branchService.Update(branch);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] Branch branch)
        {
            var result = await _branchService.Delete(branch);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }

}
