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

        [RegularExpression(@"[0-9]{2}-[0-9]{2}-[0-9]{4}", ErrorMessage = "Debe ingresar una fecha valida.")]
        public string Birthdate { get; set; } = string.Empty;
        
        public DateTime ParsedBirthdate
        {
            get
            {
                if (DateTime.TryParseExact(Birthdate, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out var date))
                {
                    if (date > DateTime.Today)
                    {
                        throw new ArgumentException("La fecha de nacimiento no puede ser mayor que la actual.");
                    }
                    return date;
                }
                throw new ArgumentException("Debe ingresar una fecha válida.");
            }
        }

        [EmailAddress]
        [Required]
        public string Email {get; set;} = null!;

        [RegularExpression(@"masculino|femenino|otro|prefiero no decirlo", ErrorMessage = "Debe ingresar una opción valida.") ]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [MaxLength(20, ErrorMessage = "Password must be less than 20 characters")]
        public string Password {get; set;} = null!;

        public bool Enabled {get; set;} = true; 
    }
}