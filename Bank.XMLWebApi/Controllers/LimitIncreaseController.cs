using Bank.Business.Abstract;
using Bank.Entity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.XMLWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var result = await _limitIncreaseService.AddLimitIncreaseRequest(limitIncrease);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("updatelimit")]
        public async Task<IActionResult> UpdateLimit([FromBody] LimitIncreaseCreateDto limitIncrease)
        {
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
