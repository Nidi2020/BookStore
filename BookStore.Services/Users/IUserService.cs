using BookStore.Common.Api;
using BookStore.Entities;
using BookStore.Models.Dtos;

namespace BookStore.Services;

public interface IUserService : IDisposable
{
    Task<ApiResult> Register(RegisterDto registerDto, CancellationToken cancellationToken);

    Task<bool> IsUserExistsByUserName(string userName);

    Task<bool> CreateRole(string roleName, User user);

    Task<User> GetUserById(int userId);

    Task<ApiResult<string>> Login(string userName, string password, CancellationToken cancellationToken);

    int GetUserScore(int age);

    Task<ApiResult> UserUpdate(int id, UserEditDto userEditDto, CancellationToken cancellationToken);

    Task<ApiResult<UserDto>> GetCurrentUser(int userId, CancellationToken cancellationToken);

}

