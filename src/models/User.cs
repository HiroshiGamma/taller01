using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.models
{
    public class User
    {
        public int Id { get; set; } 
        
        public String Rut { get; set; } = string.Empty;

        public String Nombre { get; set; } = string.Empty; 

        public DateTime FechaNacimiento { get; set; }

        public String Correo { get; set; } = string.Empty;

        public String Genero { get; set; } = string.Empty;

        public String Contrasena { get; set; } = string.Empty;


        // Entityframework relationship

        public List<Product> Products {get; } = [];

        public int RoleId { get; set; } 

        public Role Roles {get; set; } = null!; 
    }
}