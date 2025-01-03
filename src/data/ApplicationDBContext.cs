using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;
using Microsoft.AspNetCore.Identity;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Receipt>()
                .HasMany(r => r.Items)
                .WithOne()
                .HasForeignKey(ri => ri.Id)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ReceiptItem>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(ri => ri.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed roles into the database
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" },
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            // Creación de usuario administrador si no existe
            var adminId = "1";
            var adminUser = new AppUser
            {
                Id = adminId,
                UserName = "IgnacioMancilla",
                NormalizedUserName = "IGNACIOMANCILLA",
                Email = "admin@idwm.cl",
                NormalizedEmail = "ADMIN@IDWM.CL",
                EmailConfirmed = true,
                Rut = "20416699-4",
                DateOfBirth = "25-10-2000",
                Gender = "Masculino",
                Enabled = true,
                PasswordHash = new PasswordHasher<AppUser>().HashPassword(null!, "P4ssw0rd")
            };
            modelBuilder.Entity<AppUser>().HasData(adminUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "1",
                UserId = adminId 
            });
        }
        
    }

    
}