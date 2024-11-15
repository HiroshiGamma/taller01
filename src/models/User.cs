using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller01.src.models;

namespace api.src.models
{
    public class User
    {
        public int Id { get; set; } 
        
        public String Rut { get; set; } = string.Empty;

        public String Nombre { get; set; } = string.Empty; 

        public DateTime FechaNacimiento { get; set; }

        public String Correo { get; set; } = string.Empty;

        public String Contrasena { get; set; } = string.Empty;
        

        // Entityframework relationship

        public List<Product> Products {get; } = [];

        public int RoleId { get; set; } 

        public Role Role {get; set; } = null!; 

        public int EstadoId {get; set;}
        public Estado estado {get; set;} = null!;

        public int GenderId {get; set;}
        public Gender gender {get; set;} = null!;
    }
}