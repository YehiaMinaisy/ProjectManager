using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Dtos
{
    public class UserDto
    {
        public String? Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [StringLength(11)]
        public string? PhoneNumber { get; set; }
    }
}
