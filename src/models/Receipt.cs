using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;

namespace taller01.src.models
{
    public class Receipt
    {
        public required string Id {get; set;}

        public DateTime Date { get; internal set; }
        
        
        //entityframework relationship

        
        public List<Product> Products {get; set;} = [];

        
    }
}