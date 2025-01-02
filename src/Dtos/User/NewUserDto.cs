using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Bogus.DataSets;
using System.ComponentModel.DataAnnotations.Schema;

namespace taller01.src.Dtos
{
    public class NewUserDto
    {

        [Required]
        [MaxLength(255, ErrorMessage = "Name must be less than 255 characters")]
        [MinLength(8, ErrorMessage = "Name must be at least 8 characters long")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must consist of alphabetic characters only.")]
        public string Username {get; set;} = null!;

        [EmailAddress]
        [Required]
        public string Email {get; set;} = null!;
        [Required]
        public string Role { get; set; } = null!;

        [Required]
        public string Token {get; set;} = null!;

       
    }
}