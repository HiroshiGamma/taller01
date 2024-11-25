using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using api.src.models;
using Bogus;
using taller01.src.models;
using Microsoft.AspNetCore.Identity;

namespace api.src.data
{
    public class Seeders
    {   
        private static readonly string[] TopLevelDomains = { "com", "org", "net", "io", "dev" };
        
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDBContext>();


                if (!context.Products.Any())
                {
                    var productFaker = new Faker<Product>()
                        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                        .RuleFor(p => p.Type, f => f.PickRandom(new[] { "Poleras", "Gorros", "Juguetería", "Alimentación", "Libros" }))
                        .RuleFor(p => p.Price, f => f.Random.Int(1000, 99999999))
                        .RuleFor(p => p.Stock, f => f.Random.Int(0, 99999))
                        .RuleFor(p => p.ImageUrl, f => GenerateSecureRandomUrl(8,12));

                    var products = productFaker.Generate(10);
                    context.Products.AddRange(products);
                    context.SaveChanges();
                    Console.WriteLine("Productos agregados al contexto.");
                }

                if(!context.Receipts.Any())
                {
                    var receiptFaker = new Faker<Receipt>()
                        .RuleFor(r => r.Date, f => f.Date.Recent());

                    var receipts = receiptFaker.Generate(10);

                    foreach (var receipt in receipts)
                    {
                        var productIds = context.Products.Select(p => p.Id).ToList();
                        var randomProductIds = productIds.OrderBy(x => Guid.NewGuid()).Take(3).ToList();
                        receipt.Products = context.Products.Where(p => randomProductIds.Contains(p.Id)).ToList();
                    }

                    context.Receipts.AddRange(receipts);
                    context.SaveChanges();
                }

                if(!context.Users.Any())
                {
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var existingEmails = new HashSet<string>();
                    var existingRuts = new HashSet<string>();

                    // Seed admin user
                    var adminUser = new AppUser
                    {
                        UserName = "Ignacio Mancilla",
                        Email = "admin@idwm.cl",
                        Rut = "20.416.699-4",
                        DateOfBirth = new DateTime(2000, 10, 25),
                        Gender = "Masculino"
                    };
                    var adminResult = userManager.CreateAsync(adminUser, "P4ssw0rd").Result;
                    if (adminResult.Succeeded)
                    {
                        userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                    }

                    // Seed regular users
                    var userFaker = new Faker<AppUser>()
                        .RuleFor(u => u.Rut, f => GenerateUniqueRandomRut(existingRuts))
                        .RuleFor(u => u.UserName, f => f.Name.FullName())
                        .RuleFor(u => u.Email, f => GenerateUniqueRandomEmail(existingEmails))
                        .RuleFor(u => u.DateOfBirth, f => f.Date.Past(18))
                        .RuleFor(u => u.Gender, f => f.PickRandom(new[] { "Femenino", "Masculino", "Prefiero no decirlo", "Otro" }))
                        .RuleFor(u => u.PasswordHash, (f, u) => userManager.PasswordHasher.HashPassword(u, f.Internet.Password(20)));

                    var users = userFaker.Generate(10);
                    foreach (var user in users)
                    {
                        var result = userManager.CreateAsync(user).Result;
                        if (result.Succeeded)
                        {
                            userManager.AddToRoleAsync(user, "User").Wait();
                        }
                    }

                    context.SaveChanges();
                }

                context.SaveChanges();
            }
        }

        private static string GenerateUniqueRandomEmail(HashSet<string> existingEmails)
        {
            string email;
            do
            {
                email = new Faker().Internet.Email();
            } while (existingEmails.Contains(email));

            existingEmails.Add(email);
            return email;
        }

        private static string GenerateUniqueRandomRut(HashSet<string> existingRuts)
        {
            string rut;
            do
            {
                rut = GenerateRandomRut();
            } while (existingRuts.Contains(rut));

            existingRuts.Add(rut);
            return rut;
        }

        private static string GenerateRandomRut()
        {
            Random random = new Random();
            int number = random.Next(10000000, 99999999);
            char checkDigit = GenerateCheckDigit(number);
            return $"{number}-{checkDigit}";
        }

        private static char GenerateCheckDigit(int number)
        {
            int sum = 0;
            int factor = 2;

            while (number > 0)
            {
                int digit = number % 10;
                sum += digit * factor;
                factor = factor == 7 ? 2 : factor + 1;
                number /= 10;
            }

            int mod = 11 - (sum % 11);
            if (mod == 11) return '0';
            if (mod == 10) return 'K';

            return mod.ToString()[0];
        }
        
        public static string GenerateSecureRandomUrl(int subdomainLength, int pathLength)
        {
            var subdomain = GenerateCryptoRandomString(subdomainLength).ToLower();
            var tld = TopLevelDomains[RandomNumberGenerator.GetInt32(TopLevelDomains.Length)];
            var path = GenerateCryptoRandomString(pathLength).ToLower();
            
            return $"https://{subdomain}.{tld}/{path}";
        }
        
        private static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new StringBuilder(length);
            
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            
            return result.ToString();
        }
        
        private static string GenerateCryptoRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new StringBuilder(length);
            
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[RandomNumberGenerator.GetInt32(chars.Length)]);
            }
            
            return result.ToString();
        }
    }
}
