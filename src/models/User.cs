using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller01.src.models;

namespace api.src.models
{
    public class User
    {
        public int Id { get; set; } 
        
        public String Rut { get; set; } = string.Empty;

        public String Name { get; set; } = string.Empty; 

        public DateTime Birthdate { get; set; }

        public String Mail { get; set; } = string.Empty;

        public String Password { get; set; } = string.Empty;
        

        // Entityframework relationship

        public List<Product> Products {get; set;} = [];
        public List<Receipt> Receipts {get; } = [];
        

        public int RoleId { get; set; } 

        public Role Role {get; set; } = null!; 

        public int StatusId {get; set;}
        public Status Status {get; set;} = null!;

        public int GenderId {get; set;}
        public Gender Gender {get; set;} = null!;
    }
}