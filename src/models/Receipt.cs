using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;

namespace taller01.src.models
{
    public class Receipt
    {
        public int Id {get; set;}
        
        
        //entityframework relationship

        List<Product> Products {get; set;} = [];

        
    }
}