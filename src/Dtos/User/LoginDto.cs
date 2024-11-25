using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace taller01.src.Dtos
{
    public class LoginDto
    {
        [Required]
        [MaxLength(255, ErrorMessage = "Name must be less than 255 characters")]
        [MinLength(8, ErrorMessage = "Name must be at least 8 characters long")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must consist of alphabetic characters only.")]
        public string UserName { get; set; } = null!;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [MaxLength(20, ErrorMessage = "Password must be less than 20 characters")]
        public string Password { get; set; } = null!;
    }
}