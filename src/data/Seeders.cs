using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;
using Bogus;
using taller01.src.models;

namespace api.src.data
{
    public class Seeders
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDBContext>();

                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new Role
                        {
                            Nombre = "Admin"
                        },
                        new Role
                        {
                            Nombre = "User"
                        }
                    );
                    context.SaveChanges();
                }
                if (!context.Estados.Any()) 
                {
                    context.Estados.AddRange(
                        new Estado
                        {
                            Name = "Habilitado"
                        },
                        new Estado
                        {
                            Name = "Deshabilitado"
                        }
                    );
                    context.SaveChanges();
                }

                if (!context.Genders.Any()) 
                {
                    context.Genders.AddRange(
                        new Gender
                        {
                            Name = "Femenino"
                        },
                        new Gender
                        {
                            Name = "Masculino"
                        },
                        new Gender
                        {
                            Name = "Prefiero no decirlo"
                        },
                        new Gender
                        {
                            Name = "Otro"
                        }
                    );
                    context.SaveChanges();
                }

                var existingRuts = new HashSet<string>();

                if (!context.Users.Any())
                {
                    var userFaker = new Faker<User>()
                        .RuleFor(u => u.Nombre, f => f.Name.FullName())
                        .RuleFor(u => u.Rut, f => GenerateUniqueRandomRut(existingRuts))
                        .RuleFor(u => u.FechaNacimiento, f => f.Date.Past(30, DateTime.Today))
                        .RuleFor(u => u.Correo, f => f.Internet.Email())
                        .RuleFor(u => u.Contrasena, f => f.Internet.Password(8))
                        .RuleFor(u => u.RoleId, f => f.PickRandom(new[] { 1, 2 }))
                        .RuleFor(u => u.EstadoId, f => f.PickRandom(new[] { 1, 2 }))
                        .RuleFor(u => u.GenderId, f => f.PickRandom(new[] { 1, 2, 3, 4}));

                    var users = userFaker.Generate(10);
                    context.Users.AddRange(users);
                    context.SaveChanges();
                }

                if (!context.Products.Any())
                {
                    var productFaker = new Faker<Product>()
                        .RuleFor(p => p.Nombre, f => f.Commerce.ProductName())
                        .RuleFor(p => p.Tipo, f => f.PickRandom(new[] { "Poleras", "Gorros", "Juguetería", "Alimentación", "Libros" }))
                        .RuleFor(p => p.Precio, f => f.Random.Int(1000, 99999999))
                        .RuleFor(p => p.Stock, f => f.Random.Int(0, 99999));

                    var products = productFaker.Generate(10);
                    context.Products.AddRange(products);
                    context.SaveChanges();
                    Console.WriteLine("Productos agregados al contexto.");
                }

                context.SaveChanges();
            }

            
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
    }
}
