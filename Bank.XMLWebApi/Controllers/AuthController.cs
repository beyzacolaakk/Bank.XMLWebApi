using Bank.Business.Abstract;
using Bank.Core.Utilities.Results;
using Bank.Entity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.XMLWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private IAuthService _authService;
        private IUserService _userService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var result = await _authService.LoginAndCreateToken(userLoginDto);

            if (!result.Success)
                return BadRequest(result.Message);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = result.Data.Token.Expiration
            };

            Response.Cookies.Append("AuthToken", result.Data.Token.Token, cookieOptions);

            return Ok(result.Data.Token);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            var result = await _authService.Register(userRegisterDto);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("authenticate")]
        public async Task<ActionResult> Authenticate()
        {
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                var result = new ErrorResult("Unauthorized access!");
                return Unauthorized(result);
            }

            var successResult = new SuccessResult("Authentication successful.");
            return Ok(successResult);
        }

        [Authorize(Roles = "Customer,Administrator")]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            int userId = GetUserIdFromToken();
            _authService.Logout(userId);

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Path = "/"
            };

            Response.Cookies.Append("AuthToken", "", cookieOptions);

            var result = new SuccessResult("Logout completed successfully.");
            return Ok(result);
        }
    }


}
