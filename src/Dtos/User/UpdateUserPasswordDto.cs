using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace taller01.src.Dtos.User
{
    public class UpdateUserPasswordDto
    {
        [Required(ErrorMessage = "La contraseña no esta correcta")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,20}$", ErrorMessage = "Debe ingresar una contraseña valida.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña no esta correcta")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,20}$", ErrorMessage = "Debe ingresar una contraseña valida.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmación de la contraseña no esta correcta")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,20}$", ErrorMessage = "Debe ingresar una contraseña valida.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}