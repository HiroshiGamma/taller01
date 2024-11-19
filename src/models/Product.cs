using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.models
{
    public class Product
    {
        
        public int Id { get; set; } 

        public String Nombre { get; set; } = string.Empty;

        public String Tipo { get; set; } = string.Empty;

        public int Precio { get; set; }

        public int Stock { get; set; }

        public List<User> Users {get;} = new List<User>();
        
    }
}