using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace taller01.src.Dtos
{
    public class UpdateUserPasswordDto
    {
        public int Id {get; set;}
        
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{8,20}$", ErrorMessage = "Password must be alphanumeric and between 8 to 20 characters.")]
        public string Password {get; set;} = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]{8,255}$", ErrorMessage = "Nombre must be between 8 and 255 characters and only contain Spanish alphabet characters.")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^(femenino|masculino|prefiero no decirlo|otro)$", ErrorMessage = "Genero must be 'femenino', 'masculino', 'prefiero no decirlo', or 'otro'.")]
        public string Genero { get; set; } = string.Empty;
    }
}