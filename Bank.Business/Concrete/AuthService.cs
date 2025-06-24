using Bank.Business.Abstract;
using Bank.Business.Constant;
using Bank.Core.Entities.Concrete;
using Bank.Core.Utilities.Results;
using Bank.Core.Utilities.Security.Hashing;
using Bank.Core.Utilities.Security.JWT;
using Bank.Entity.Concrete;
using Bank.Entity.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Business.Concrete
{
    public class AuthService : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private IUserRoleService _userRoleService;
        private ILoginEventService _loginEventService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ILoginTokenService _loginTokenService;
        private readonly IMemoryCache _memoryCache;

        public AuthService(IUserService userService, ILoginTokenService loginTokenService, ITokenHelper tokenHelper, IHttpContextAccessor httpContextAccessor, IUserRoleService userRoleService, ILoginEventService loginEventService, IMemoryCache memoryCache)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _userRoleService = userRoleService;
            _loginEventService = loginEventService;
            _httpContextAccessor = httpContextAccessor;
            _loginTokenService = loginTokenService;
            _memoryCache = memoryCache;
        }

        public async Task<IDataResult<AccessToken>> CreateAccessToken(IDataResult<User> user)
        {
            var claims = await _userService.GetRoles(user.Data);
            var accessToken = _tokenHelper.CreateToken(user.Data, claims);
            await _loginTokenService.Add(new LoginToken
            {
                UserId = user.Data.Id,
                ExpirationDate = accessToken.Expiration,
                Token = accessToken.Token,
                CreatedDate = DateTime.UtcNow
            });

            return new SuccessDataResult<AccessToken>(accessToken, user.Message);
        }

        public async Task<IResult> Register(UserRegisterDto dto)
        {
            var exists = await UserExists(dto.Phone);
            if (!exists.Success)
                return new ErrorDataResult<AccessToken>(exists.Message);

            HashingHelper.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var newUser = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                Phone = dto.Phone,
                BranchId = dto.Branch,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RegistrationDate = DateTime.UtcNow,
                Active = true
            };

            var addResult = await _userService.Add(newUser);
            if (!addResult.Success)
                return new ErrorDataResult<AccessToken>(Messages.UserAddFailed);

            var roleResult = await AddUserRole(new SuccessDataResult<User>(newUser));
            if (!roleResult.Success)
                return new ErrorDataResult<AccessToken>(roleResult.Message);

            return new SuccessResult(Messages.RegistrationSuccessful);
        }

        public void Logout(int id)
        {
            _memoryCache.Remove($"user_{id}");
        }

        public async Task<IDataResult<UserAndTokenDto>> LoginAndCreateToken(UserLoginDto userLoginDto)
        {
            var user = await _userService.GetByPhone(userLoginDto.Phone);
            if (user == null)
            {
                return new ErrorDataResult<UserAndTokenDto>(Messages.InvalidInformation);
            }

            var isPasswordValid = HashingHelper.VerifyPasswordHash(
                userLoginDto.Password,
                user.PasswordHash,
                user.PasswordSalt
            );

            if (!isPasswordValid)
            {
                await TrackLogin(user, false, userLoginDto.IpAddress);
                return new ErrorDataResult<UserAndTokenDto>(Messages.InvalidInformation);
            }

            var tokenResult = await CreateAccessToken(new SuccessDataResult<User>(user));
            if (!tokenResult.Success)
            {
                await TrackLogin(user, false, userLoginDto.IpAddress);
                return new ErrorDataResult<UserAndTokenDto>(tokenResult.Message);
            }

            await TrackLogin(user, true, userLoginDto.IpAddress);

            var dto = new UserAndTokenDto
            {
                Token = tokenResult.Data
            };

            return new SuccessDataResult<UserAndTokenDto>(dto, Messages.LoginSuccessful);
        }

        private async Task TrackLogin(User user, bool isSuccess, string ipAddress)
        {
            _memoryCache.Remove("LoginEventList");
            await _loginEventService.Add(new LoginEvent
            {
                UserId = user.Id,
                IpAddress = ipAddress,
                IsSuccessful = isSuccess,
                Timestamp = DateTime.Now
            });
        }

        public async Task<IResult> UserExists(string phone)
        {
            if (await _userService.GetByPhone(phone) != null)
            {
                return new ErrorResult(Messages.AlreadyExists);
            }
            return new SuccessResult();
        }

        public async Task<IResult> AddUserRole(IDataResult<User> user)
        {
            var userRole = new UserRole
            {
                UserId = user.Data.Id,
                RoleId = 1
            };
            var result = await _userRoleService.Add(userRole);
            if (!result.Success)
            {
                return new ErrorResult(Messages.AddFailed);
            }
            return new SuccessResult();
        }
    }

}
