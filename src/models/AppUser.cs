using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace taller01.src.models
{
    public class AppUser : IdentityUser
    {
        public string Rut {get; set;} = string.Empty;

        public string DateOfBirth { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public bool Enabled {get; set;} 

    }
}