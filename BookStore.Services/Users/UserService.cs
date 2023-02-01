using BookStore.Common.Api;
using BookStore.Common.Constants;
using BookStore.Common.Constants.Strings;
using BookStore.Common.Utilities;
using BookStore.Entities;
using BookStore.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BookStore.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> userManager;
    private readonly RoleManager<Role> roleManager;
    private readonly ILogger<User> logger;
    private readonly IJwtService jwtService;
    private readonly IMemoryCache memoryCache;
    public UserService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<User> logger,
        IJwtService jwtService,
        IMemoryCache memoryCache
        )
    {
        this.logger = logger;
        this.jwtService = jwtService;
        this.memoryCache = memoryCache;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task<ApiResult> Register(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        try
        {
            if (await IsUserExistsByUserName(registerDto.Email))
                return new ApiResult(false, ApiResultStatusCode.BadRequest, UserStrings.DuplicateUserName);


            var user = new User
            {
                Name = registerDto.Name,
                Family = registerDto.Family,
                Age = registerDto.Age,
                BirthDate = registerDto.BirthDate,
                NationalCode = registerDto.NationalCode,
                Mobile = registerDto.Mobile,
                Email = registerDto.Email,
                UserName = registerDto.Email,
            };

            var addUser = await userManager.CreateAsync(user, registerDto.Password);
            if (!addUser.Succeeded)
                return new ApiResult(false, ApiResultStatusCode.BadRequest, UserStrings.UsernameAndPasswordIncorrect);

            var role = await CreateRole("User", user);
            if (!role)
                return new ApiResult(false, ApiResultStatusCode.ServerError, UserStrings.RoleFailed);

            return new ApiResult(true, ApiResultStatusCode.Success, UserStrings.UserSuccessRegistered);

        }
        catch (Exception ex)
        {
            logger.LogError("Error in Register Api! " + ex.Message);
            return new ApiResult(false, ApiResultStatusCode.ServerError, CommonStrings.ErrorMesssage);
        }
    }
    public async Task<bool> IsUserExistsByUserName(string userName)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user != null)
            return true;

        return false;
    }
    public async Task<bool> CreateRole(string roleName, User user)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            await roleManager.CreateAsync(new Role
            {
                Name = roleName,
                Description = "",
                NormalizedName = roleName.ToLower(),
            });
        }

        var addRole = await userManager.AddToRoleAsync(user, roleName);
        if (!addRole.Succeeded)
            return false;

        return true;

    }

    public async Task<User> GetUserById(int userId)
    {
        return await userManager.FindByIdAsync(userId.ToString());
    }
    public async Task<ApiResult<string>> Login(string userName, string password, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
            return new ApiResult<string>(false, ApiResultStatusCode.BadRequest, UserStrings.UsernameAndPasswordIncorrect);

        var isPasswordValid = await userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid)
            return new ApiResult<string>(false, ApiResultStatusCode.BadRequest, UserStrings.UsernameAndPasswordIncorrect);

        var jwt = await jwtService.GenerateAsync(user);

        return new ApiResult<string>(true, ApiResultStatusCode.Success, jwt, UserStrings.SuccessMessage);
    }
    public int GetUserScore(int age)
    {
        return age switch
        {
            (>= 15 and <= 25) => 40,
            (> 25 and <= 35) => 30,
            (> 35 and <= 45) => 20,
            (> 45 and <= 55) => 10,
            (> 55) => 5,
            _ => 0
        };
    }
    public async Task<ApiResult> UserUpdate(int id, UserEditDto userEditDto, CancellationToken cancellationToken)
    {
        var updateUser = await GetUserById(id);
        if (updateUser == null)
            return new ApiResult(false, ApiResultStatusCode.NotFound, UserStrings.UserNotFound);

        if (!string.IsNullOrEmpty(userEditDto.Password))
        {
            var passwordHash = SecurityHelper.GetSha256Hash(userEditDto.Password);
            updateUser.PasswordHash = passwordHash;
        }
        if (!string.IsNullOrEmpty(userEditDto.Name))
            updateUser.Name = userEditDto.Name;

        if (!string.IsNullOrEmpty(userEditDto.Family))
            updateUser.Family = userEditDto.Family;

        if (userEditDto.Age != null)
            updateUser.Age = Convert.ToInt32(userEditDto.Age);

        if (userEditDto.BirthDate != null)
            updateUser.BirthDate = userEditDto.BirthDate;

        if (!string.IsNullOrEmpty(userEditDto.NationalCode))
            updateUser.NationalCode = userEditDto.NationalCode;

        if (!string.IsNullOrEmpty(userEditDto.Mobile))
            updateUser.Mobile = userEditDto.Mobile;

        if (!string.IsNullOrEmpty(userEditDto.Email))
        {
            updateUser.Email = userEditDto.Email;
            updateUser.UserName = userEditDto.Email;
        }

        await userManager.UpdateAsync(updateUser);

        return new ApiResult(true, ApiResultStatusCode.Success, CommonStrings.SuccessMessage);

    }
    public async Task<ApiResult<UserDto>> GetCurrentUser(int userId, CancellationToken cancellationToken)
    {
        if (!memoryCache.TryGetValue("UserInfo", out UserDto userDto))
        {
            var user = await GetUserById(userId);
            userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Family = user.Family,
                Age = user.Age,
                BirthDate = user.BirthDate,
                Email = user.Email,
                Mobile = user.Mobile,
                IsActive = user.IsActive,
                NationalCode = user.NationalCode
            };
            var expirationOptions = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.Normal,
                AbsoluteExpiration = DateTime.Now.AddMinutes(60)
            };
            memoryCache.Set("UserInfo", userDto, expirationOptions);
        }

        return new ApiResult<UserDto>(true, ApiResultStatusCode.Success, userDto, CommonStrings.SuccessMessage);
    }
    public void Dispose()
    {
        userManager.Dispose();
        roleManager.Dispose();
    }
}

