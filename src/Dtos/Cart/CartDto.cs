using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Dtos.Cart
{
    public class CartDto
    {
        /// <summary>
        ///  Lista de items en el carrito
        ///  </sumary> 
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        /// <summary>
        ///  Total de la comrpa
        ///  </sumary> 
        public int Total { get; set; }
    }
}