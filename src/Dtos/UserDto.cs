using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taller01.src.Dtos
{
    public class UserDto
    {
        public int Id { get; set; } 
        public String Rut { get; set; } = string.Empty;

        public String Nombre { get; set; } = string.Empty; 

        public DateTime FechaNacimiento { get; set; }

        public String Correo { get; set; } = string.Empty;

        public int GenderId {get; set;}

        public int EstadoId {get; set;}

    }
}