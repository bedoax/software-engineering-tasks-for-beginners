﻿using System.ComponentModel.DataAnnotations;

namespace BackendTask3.Model
{
    public class Login
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
