using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace taller01.src.Dtos
{
    public class UpdateUserDto
    {

        [StringLength(255,MinimumLength = 8,ErrorMessage = "Name must be between 8 and 255 characters.")]
        public string Name { get; set; } = string.Empty;


        [RegularExpression(@"[0-9]{2}-[0-9]{2}-[0-9]{4}", ErrorMessage = "Debe ingresar una fecha valida.")]
        public string Birthdate { get; set; } = string.Empty;
        /// <summary>
        /// Metodo para parsear la fecha de nacimiento, verificando que sea una fecha valida y que no sea una fecha futura
        /// </summary>
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


        [RegularExpression(@"masculino|femenino|otro|prefiero no decirlo", ErrorMessage = "Choose a valid option.") ]
        public string Gender { get; set; } = string.Empty;
    }
}