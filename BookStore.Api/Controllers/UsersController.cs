using BookStore.Common.Api;
using BookStore.Common.Constants;
using BookStore.Common.Utilities;
using BookStore.Models.Dtos;
using BookStore.Services;
using BookStore.WebFramework.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiResultFilter]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ApiResult> Register(RegisterDto registerDto, CancellationToken cancellationToken)
          => await userService.Register(registerDto, cancellationToken);

       
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<ApiResult<string>> Login([FromQuery] LoginDto loginDto, CancellationToken cancellationToken)
          => await userService.Login(loginDto.Email, loginDto.Password, cancellationToken);

        [HttpPut]
        public async Task<ApiResult> Update(UserEditDto userEditDto, CancellationToken cancellationToken)
        {
            var userId = User.Identity?.GetUserId<int>() ?? null;
            if (userId == null)
                return new ApiResult(false, ApiResultStatusCode.UnAuthorized, CommonStrings.UnAuthorizedMessage);

            return await userService.UserUpdate((int)userId, userEditDto, cancellationToken);
        }

        [HttpGet]
        public async Task<ApiResult> Get(CancellationToken cancellationToken)
        {
            var userId = User.Identity?.GetUserId<int>() ?? null;
            if (userId == null)
                return new ApiResult(false, ApiResultStatusCode.UnAuthorized, CommonStrings.UnAuthorizedMessage);

            return await userService.GetCurrentUser((int)userId, cancellationToken);
        }
    }
}
