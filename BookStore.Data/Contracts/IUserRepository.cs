using BookStore.Entities;

namespace BookStore.Data.Contracts;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByUserAndPassAsync(string username, string password, CancellationToken cancellationToken);
    Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
}

