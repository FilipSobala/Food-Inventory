using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace FoodInventory.Models
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
           

          
            if (!context.Users.Any(u => u.Role == "Admin"))
            {
                var hasher = new PasswordHasher<User>();
                var adminUser = new User
                {
                    Username = "admin",
                    Role = "Admin",
                    ApiToken = "ADMIN-SUPER-SECRET-TOKEN-123"
                };
                adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin123");

                context.Users.Add(adminUser);
            }

            
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Nabiał", Icon = "🧀" },
                    new Category { Name = "Warzywa i Owoce", Icon = "🥦" },
                    new Category { Name = "Mięso", Icon = "🥩" },
                    new Category { Name = "Pieczywo", Icon = "🍞" },
                    new Category { Name = "Inne", Icon = "📦" }
                );
            }

            context.SaveChanges();
        }
    }
}