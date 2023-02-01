using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Entities;

public class Category : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; }
    public string Description { get; set; }

    [DefaultValue(true)]
    public bool IsActive { get; set; }
    public int? ParentId { get; set; }
   
    [ForeignKey(nameof(ParentId))] // or [ForeignKey("ParentId")]
    public List<Category> Parents { get; set; }
    public virtual List<Book> Books { get; set; }
}


