using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.models
{
    public class Product
    {
        
        public int Id { get; set; } 

        public String Name { get; set; } = string.Empty;

        public String Type { get; set; } = string.Empty;

        public int Price { get; set; }

        public int Stock { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public List<User> Users {get;} = new List<User>();
        
    }
}