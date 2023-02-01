using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;
namespace BookStore.Entities;

public class User : IdentityUser<int>, IEntity
{
    public string? Name { get; set; }
    public string? Family { get; set; }
    public int Age { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? NationalCode { get; set; }
    public string? Mobile { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset LastLoginDate { get; set; }
    public ICollection<Order> Orders { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(p => p.UserName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.PasswordHash).IsRequired().HasMaxLength(500);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Family).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Age).IsRequired();
        builder.Property(p => p.NationalCode).HasMaxLength(10);
        builder.Property(p => p.Mobile).HasMaxLength(50);
        builder.Property(p => p.Email).HasMaxLength(500);
        builder.Property(p => p.IsActive).HasDefaultValue(true);
    }
}

