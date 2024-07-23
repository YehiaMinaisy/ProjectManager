﻿using System.ComponentModel.DataAnnotations;

namespace ProjectManagerWebAPI.Dtos.User
{
    public class LoginDto
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
