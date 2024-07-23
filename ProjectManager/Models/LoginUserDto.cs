﻿using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models
{
    public class LoginUserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
