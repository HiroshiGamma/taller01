using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Dtos
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public int Price { get; set; }

        public int Stock { get; set; }
    }
}