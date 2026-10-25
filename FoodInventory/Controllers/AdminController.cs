using FoodInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace FoodInventory.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        
        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";

        
        public IActionResult Products()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var products = _context.Products.Include(p => p.Category).ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult AddProduct(string name, int categoryId)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var product = new Product { Name = name, CategoryId = categoryId };
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Products");
        }

        
        public IActionResult Categories()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var categories = _context.Categories.ToList();
            return View(categories);
        }

        [HttpPost]
        public IActionResult AddCategory(string name, string icon)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var category = new Category { Name = name, Icon = icon };
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Categories");
        }

      
        public IActionResult Users()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var users = _context.Users.ToList();
            return View(users);
        }

        
        public IActionResult Recipes()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var recipes = _context.Recipes.Include(r => r.RecipeProducts).ThenInclude(rp => rp.Product).ToList();
            ViewBag.Products = _context.Products.ToList();
            return View(recipes);
        }

        [HttpPost]
        public IActionResult AddRecipe(string name, string instructions, int prepTimeMinutes, int productId, double quantity, string unit)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var recipe = new Recipe
            {
                Name = name,
                Instructions = instructions,
                PrepTimeMinutes = prepTimeMinutes
            };

            _context.Recipes.Add(recipe);
            _context.SaveChanges();

            
            var recipeProduct = new RecipeProduct
            {
                RecipeId = recipe.Id,
                ProductId = productId,
                Quantity = quantity,
                Unit = unit
            };

            _context.RecipeProducts.Add(recipeProduct);
            _context.SaveChanges();

            return RedirectToAction("Recipes");
        }
    }
}

