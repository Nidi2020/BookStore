using BookStore.Entities;

namespace BookStore.Services;

public interface IJwtService
{
    Task<string> GenerateAsync(User user);
}

