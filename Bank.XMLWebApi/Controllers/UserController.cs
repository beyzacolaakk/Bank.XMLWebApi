using Bank.Business.Abstract;
using Bank.Core.Extensions;
using Bank.Core.Utilities.Results;
using Bank.Core.Utilities.XMLSerializeToXML;
using Bank.Entity.DTOs;
using Bank.XMLWebApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.XMLWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/xml")]
    [Consumes("application/xml")]
    public class UserController : BaseController
    {
        private IUserService _userService;
        private IBranchService _branchService; 

        public UserController(IUserService userService,IBranchService branchService)
        {
            _userService = userService;
            _branchService = branchService;
        }

        [HttpGet("getuser")]
        public async Task<IActionResult> GetFilteredUserXml()
        {
            var userId = GetUserIdFromToken();
            var result = await _userService.GetById(userId);
            var branchinf= await _branchService.GetById(result.Data.BranchId);
            if (!result.Success)
                return BadRequest(result.Message);

            string fullXml = XmlHelper.SerializeToXml(result.Data);

        
            var (FullName, Email, Phone) = UserParse.ParseUserInfoFromXml(fullXml);  


            string filteredXml = $@"
<UserInfoDto>
  <fullName>{FullName}</fullName>
  <email>{Email}</email>
  <phone>{Phone}</phone>
<branch>{branchinf.Data.BranchName}</branch>
</UserInfoDto>";
            var res= XmlConverter.Deserialize<UserInfoDto>(filteredXml);
            var rese= new SuccessDataResult<UserInfoDto>(res,"Success");
            if (result.Success)
                return Ok(rese);
            return BadRequest(rese);
        }



    }

}
