using System.ComponentModel.DataAnnotations;

namespace MovieApp.Server.DTO
{
    public class RegisterUserDTO
    {
        [Required]
        [MaxLength(40)]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
