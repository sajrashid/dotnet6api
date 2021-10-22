using System.ComponentModel.DataAnnotations;

namespace MyAPI.Models
{
    public class Auth
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(24, ErrorMessage = "Password must be a Minimuo of 6 and less that 24 characters.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Password Must Contain at least 1 upper case char and a number.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; } = null!;
    }
}
