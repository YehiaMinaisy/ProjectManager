using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "This filled must be populated")]
        [MaxLength(80)]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        //[PasswordPropertyText]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        [StringLength(11)]
        public string? PhoneNumber { get; set; }
    }
}
