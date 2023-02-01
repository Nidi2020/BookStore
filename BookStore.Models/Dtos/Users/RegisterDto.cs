using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Dtos;

public class RegisterDto : IValidatableObject
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string Family { get; set; }

    [Required]
    public int Age { get; set; }

    public DateTime? BirthDate { get; set; }

    [StringLength(10)]
    public string NationalCode { get; set; }

    [StringLength(50)]
    public string Mobile { get; set; }


    [Required]
    [StringLength(500)]
    public string Email { get; set; }

    [Required]
    [StringLength(500)]
    public string Password { get; set; }


    /// <summary>
    /// business errors
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Email.Equals("test", StringComparison.OrdinalIgnoreCase))
            yield return new ValidationResult("Username cannot be test!", new[] { nameof(Email) });
        //if (Password.Equals("123456"))
        //    yield return new ValidationResult("The password cannot be 123456!", new[] { nameof(Password) });
    }
}

