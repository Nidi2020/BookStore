using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; }
    public string Code { get; set; }
    public int CategoryId { get; set; }   
    public Category Category { get; set; }  //nagigation property
    public string Picture { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int Price { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public string Publication { get; set; }
    public string Subject { get; set; }
    public string Translator { get; set; }
    public string Language { get; set; }
    public string PublishYear { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }
}

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Code).HasMaxLength(50);
        builder.Property(p => p.CategoryId).IsRequired();        
        builder.Property(p => p.Picture).HasMaxLength(500);
        builder.Property(p => p.Price).IsRequired();
        builder.Property(p => p.Author).HasMaxLength(100);
        builder.Property(p => p.ISBN).HasMaxLength(100);
        builder.Property(p => p.Publication).HasMaxLength(100);
        builder.Property(p => p.Subject).HasMaxLength(100);
        builder.Property(p => p.Translator).HasMaxLength(100);
        builder.Property(p => p.Language).HasMaxLength(100);
        builder.Property(p => p.PublishYear).HasMaxLength(50);
        builder.Property(p => p.IsActive).HasDefaultValue(true);

        builder.HasOne(p => p.Category).WithMany(p => p.Books).HasForeignKey(p => p.CategoryId);

    }
}

