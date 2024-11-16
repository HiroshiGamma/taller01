using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taller01.src.Dtos
{
    public class UpdateUserPasswordDto
    {
        public int Id {get; set;}
        public required string Contrasena {get; set;}
    }
}