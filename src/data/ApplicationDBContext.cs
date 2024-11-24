using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using taller01.src.models;

namespace api.src.data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions): base(dbContextOptions) 
        {
            
        }
        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Receipt> Receipts { get; set; } = null!;
    }

    
}