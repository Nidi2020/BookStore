using BookStore.Common.Utilities;
using BookStore.Data.Contracts;
using BookStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<User> GetByUserAndPassAsync(string username, string password, CancellationToken cancellationToken)
    {
        var passwordHash = SecurityHelper.GetSha256Hash(password);
        var user = await Table.Where(p => p.UserName == username && p.PasswordHash == passwordHash).SingleOrDefaultAsync(cancellationToken);
        return user;
    }
    public async Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
    {
        user.LastLoginDate = DateTimeOffset.Now;
        await UpdateAsync(user, cancellationToken);
    }
}

