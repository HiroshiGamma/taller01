using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.models
{
    public class ReceiptItem
    {

        public int Id { get; set; }


        public int ProductId { get; set; }


        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }


        public int Price { get; set; }

        public int Total { get; set; }
    }
}