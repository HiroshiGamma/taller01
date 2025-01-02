using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Dtos.Cart
{
    public class CartItemDto
    {
            
        /// <summary>
        /// Producto del carrito
        /// </summary> 
        public int ProductId { get; set; }

        /// <summary>
        /// Nombre del producto
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad del producto
        /// </summary>
        public int Quantity { get; set; }
        
        /// <summary>
        /// Precio del producto
        /// </summary>
        public int Price { get; set; }
    }
}