using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.models
{
    public class Cart
    {
        /// <summary>
        /// Id del carrito
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Lista de items en el carrito
        /// </summary> 
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}