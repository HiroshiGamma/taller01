using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;
using Microsoft.EntityFrameworkCore;

namespace api.src.data
{
    public class ApplicationDBContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Role> Roles { get; set; } = null!;


    }
}