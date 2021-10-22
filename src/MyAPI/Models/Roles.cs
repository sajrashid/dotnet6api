using System.ComponentModel.DataAnnotations;

namespace MyAPI.Models
{
    public class Roles
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(16)]
        public string? Role { get; set; }
    }
}
