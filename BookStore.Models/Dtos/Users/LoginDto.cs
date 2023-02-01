using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Dtos;

public class LoginDto
{
    [Display(Name = "Email")]
    [Required(ErrorMessage = "Enter {0}")]
    [StringLength(500, ErrorMessage = "{0} must be at least {2} characters")]
    public string Email { get; set; }

    [Display(Name = "Password")]
    [Required(ErrorMessage = "Enter {0}")]
    [StringLength(500, ErrorMessage = "{0} must be at least {2} characters", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }


}

