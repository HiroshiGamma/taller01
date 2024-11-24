using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taller01.src.Dtos
{
    public class NewUserDto
    {
        public string Username {get; set;} = null!;
        public string Email {get; set;} = null!;
        public string Token {get; set;} = null!;
    }
}