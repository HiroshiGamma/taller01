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
        [MaxLength(255, ErrorMessage = "Name must be less than 255 characters")]
        [MinLength(8, ErrorMessage = "Name must be at least 8 characters long")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must consist of alphabetic characters only.")]
        public string Username {get; set;} = null!;
        
        [MaxLength(10, ErrorMessage = "Rut must be less than 10 characters")]
        [Required]
        public string Rut {get; set;} = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate 
        {
            get => _birthdate;
            set
            {
                if (value >= DateTime.Now)
                {
                    throw new ValidationException("Birthdate must be a date before the current date.");
                }
                _birthdate = value;
            }
        }
        public DateTime _birthdate;

        [EmailAddress]
        [Required]
        public string Email {get; set;} = null!;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [MaxLength(20, ErrorMessage = "Password must be less than 20 characters")]
        public string Password {get; set;} = null!;

        public bool enabled {get; set;} = true; 
    }
}