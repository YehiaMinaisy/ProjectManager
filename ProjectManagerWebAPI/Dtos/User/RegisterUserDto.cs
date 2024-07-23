using System.ComponentModel.DataAnnotations;

namespace ProjectManagerWebAPI.Dtos.User
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "This filled must be populated")]
        [MaxLength(80)]
        public string? UserName { get; set; }
        [Required]

        public string? Password { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        [StringLength(11)]
        public string? PhoneNumber { get; set; }
    }
}
