using Bank.Business.Abstract;
using Bank.Core.Utilities.XMLSerializeToXML;
using Bank.XMLWebApi.Helpers;
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

        [HttpGet("getuser")]
        public async Task<IActionResult> GetFilteredUserXml()
        {
            var userId = GetUserIdFromToken();
            var result = await _userService.GetById(userId);

            if (!result.Success)
                return BadRequest(result.Message);

            // XML'e çevir
            string fullXml = XmlHelper.SerializeToXml(result.Data);

            // XML içinden sadece gerekli alanları çe
            var (FullName, Email, Phone) = UserParse.ParseUserInfoFromXml(fullXml);  

            // Yeni sade XML oluştur
            string filteredXml = $@"
<User>
  <FullName>{FullName}</FullName>
  <Email>{Email}</Email>
  <Phone>{Phone}</Phone>
</User>";

            return Content(filteredXml, "application/xml");
        }



    }

}
