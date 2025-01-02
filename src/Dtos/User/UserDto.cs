using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Dtos.User
{
    public class UserDto
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// RUT del usuario. Debe ser un RUT válido.
        /// </summary>
        [RegularExpression(@"[0-9]{8}-[0-9]{1}", ErrorMessage = "Please enter a valid RUT.")]
        [Required(ErrorMessage = "RUT is required.")]
        [Key]
        public string Rut { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del usuario. Debe tener entre 8 y 255 caracteres.
        /// </summary>
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Name must be between 8 and 255 characters.")]
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

        /// <summary>
        /// Correo electrónico del usuario. Es obligatorio.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Mail { get; set; } = string.Empty;

        /// <summary>
        /// Género del usuario. Debe ser una opción válida.
        /// </summary>
        [RegularExpression(@"masculino|femenino|otro|prefiero no decirlo", ErrorMessage = "Please enter a valid option.")]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario. Debe ser una contraseña válida.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,20}$", ErrorMessage = "Please enter a valid password.")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el usuario está habilitado.
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}