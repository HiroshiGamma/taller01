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

        public DateTime Date { get; internal set; }
        
        
        //entityframework relationship

        public int UserId {get; set;}

        public User User {get; set;} = null!;
        
        public List<Product> Products {get; set;} = [];

        
    }
}