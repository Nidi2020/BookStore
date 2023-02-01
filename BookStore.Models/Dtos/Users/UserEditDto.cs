using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Dtos;
public class UserEditDto
{

    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(100)]
    public string Family { get; set; }
    public int? Age { get; set; }
    public DateTime? BirthDate { get; set; }

    [StringLength(10)]
    public string NationalCode { get; set; }

    [StringLength(50)]
    public string Mobile { get; set; }

    [StringLength(500)]
    public string Email { get; set; }

    [StringLength(500)]
    public string Password { get; set; }

}
