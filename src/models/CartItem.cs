using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.models
{
    public class CartItem
    {

        public int Id { get; set; }

        public int ProductId { get; set; }

        public required Product Product { get; set; }

        public int Quantity { get; set; }

    }
}