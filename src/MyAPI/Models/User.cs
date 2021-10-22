using System.ComponentModel.DataAnnotations;

namespace MyAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public String? Email { get; set; }

        [Required]
        public byte[]? Salt { get; set; }
        [Required]
        public string? Hash { get; set; }

        [Required]
        public DateTime? LastVisit { get; set; }
    }
}