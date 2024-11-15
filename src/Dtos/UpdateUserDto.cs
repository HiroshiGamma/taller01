using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taller01.src.Dtos
{
    public class UpdateUserDto
    {
        public String Nombre { get; set; } = string.Empty; 

        public DateTime FechaNacimiento { get; set; }
        public int GenderId { get; set; }
    }
}