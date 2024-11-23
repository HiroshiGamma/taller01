using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taller01.src.Dtos
{
    public class ProductDto
    {
        public int Id {get; set;}
        public String Name { get; set; } = string.Empty;
        public String Type { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Stock { get; set; }
    }
}