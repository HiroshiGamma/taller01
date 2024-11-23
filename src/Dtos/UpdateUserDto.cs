using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taller01.src.Dtos
{
    public class UpdateUserDto
    {
        public String Name { get; set; } = string.Empty; 
        public DateTime Birthdate { get; set; }
        public int GenderId { get; set; }
    }
}