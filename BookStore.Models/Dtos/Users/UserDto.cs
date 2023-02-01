using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Dtos;

public class UserDto : IValidatableObject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Family { get; set; }
    public int Age { get; set; }
    public DateTime? BirthDate { get; set; }
    public string NationalCode { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }

    /// <summary>
    /// business errors
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Name.Equals("test", StringComparison.OrdinalIgnoreCase))
            yield return new ValidationResult("Can not test for name!", new[] { nameof(Name) });
    }
}

