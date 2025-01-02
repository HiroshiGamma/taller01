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


        private DateTime dateOfBirth;

        /// <summary>
        /// Fecha de nacimiento del usuario en formato dd-MM-yyyy.
        /// </summary>
        [RegularExpression(@"[0-9]{2}-[0-9]{2}-[0-9]{4}", ErrorMessage = "Please enter a valid date.")]
        public DateTime DateOfBirth 
        { 
            get => dateOfBirth; 
            set
            {
                if (value > DateTime.Today)
                {
                    throw new ArgumentException("Date of birth cannot be in the future.");
                }
                dateOfBirth = value;
            }
        }


        [RegularExpression(@"masculino|femenino|otro|prefiero no decirlo", ErrorMessage = "Choose a valid option.") ]
        public string Gender { get; set; } = string.Empty;
    }
}