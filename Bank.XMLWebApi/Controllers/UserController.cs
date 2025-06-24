using Bank.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.XMLWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser()
        {
            var userId = GetUserIdFromToken();
            var result = await _userService.GetUserDetails(userId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

   
    }

}
