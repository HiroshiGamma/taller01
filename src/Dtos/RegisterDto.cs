using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace taller01.src.Dtos
{
     public class RegisterDto
    {
        [Required]
        public string? Name { get; set; } = string.Empty;

        [Required]
        public string? Rut { get; set; } = string.Empty;

        [EmailAddress]
        [Required]
        public string? Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string? Password { get; set; } = string.Empty;
    }
}