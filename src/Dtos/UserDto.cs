using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using System.Text.RegularExpressions;

namespace taller01.src.Dtos
{
    public class UserDto
    {
        public int Id { get; set; } 
        [Required]
        [MaxLength(12, ErrorMessage = "Rut must be 12 characters")]
        public String Rut { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]{8,255}$", ErrorMessage = "Name must only contain Spanish alphabet characters")]
        public String Name { get; set; } = string.Empty; 

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public String Mail { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{8,20}$", ErrorMessage = "Password must be alphanumeric and between 8 and 20 characters")]
        public String Password { get; set; } = string.Empty;

        [Required]
        [Range(1, 4, ErrorMessage = "Gender must be selected from the list")]
        public int GenderId { get; set; }

        public int StatusId { get; set; }


    }
}