using FoodInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace FoodInventory.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        
        private int? GetCurrentUserId() => HttpContext.Session.GetInt32("UserId");

       
        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var userProducts = _context.UserProducts
                .Include(up => up.Product)
                .ThenInclude(p => p.Category)
                .Where(up => up.UserId == userId)
                .ToList();

           
            ViewBag.AllProducts = _context.Products.ToList();
            return View(userProducts);
        }

        
        [HttpPost]
        public IActionResult AddProduct(int productId, double quantity, string unit, DateTime expiryDate, string notes)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var userProduct = new UserProduct
            {
                UserId = userId.Value,
                ProductId = productId,
                Quantity = quantity,
                Unit = unit,
                ExpiryDate = expiryDate,
                Notes = notes
            };

            _context.UserProducts.Add(userProduct);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        
        public IActionResult RemoveProduct(int id)
        {
            var userId = GetCurrentUserId();
            var up = _context.UserProducts.FirstOrDefault(x => x.Id == id && x.UserId == userId);
            if (up != null)
            {
                _context.UserProducts.Remove(up);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        
        public IActionResult ShoppingList()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var list = _context.ShoppingLists
                .Include(sl => sl.Product)
                .Where(sl => sl.UserId == userId)
                .ToList();

            ViewBag.AllProducts = _context.Products.ToList();
            return View(list);
        }

        [HttpPost]
        public IActionResult AddToShoppingList(int productId, double quantity, string unit)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            var item = new ShoppingList
            {
                UserId = userId.Value,
                ProductId = productId,
                Quantity = quantity,
                Unit = unit,
                IsBought = false
            };

            _context.ShoppingLists.Add(item);
            _context.SaveChanges();
            return RedirectToAction("ShoppingList");
        }

        public IActionResult ToggleBought(int id)
        {
            var userId = GetCurrentUserId();
            var item = _context.ShoppingLists.FirstOrDefault(sl => sl.Id == id && sl.UserId == userId);
            if (item != null)
            {
                item.IsBought = !item.IsBought;
                _context.SaveChanges();
            }
            return RedirectToAction("ShoppingList");
        }

        public IActionResult ClearBought()
        {
            var userId = GetCurrentUserId();
            var boughtItems = _context.ShoppingLists.Where(sl => sl.UserId == userId && sl.IsBought);
            _context.ShoppingLists.RemoveRange(boughtItems);
            _context.SaveChanges();
            return RedirectToAction("ShoppingList");
        }

        
        public IActionResult Recipes()
        {
            var userId = GetCurrentUserId();
            if (userId == null) return RedirectToAction("Login", "Account");

            
            var ownedProductIds = _context.UserProducts
                .Where(up => up.UserId == userId)
                .Select(up => up.ProductId)
                .Distinct()
                .ToList();

            
            var allRecipes = _context.Recipes
                .Include(r => r.RecipeProducts)
                .ThenInclude(rp => rp.Product)
                .ToList();

            
            var matchedRecipes = allRecipes.Where(r => 
                r.RecipeProducts.Any(rp => ownedProductIds.Contains(rp.ProductId))
            ).ToList();

            
            if (!ownedProductIds.Any())
            {
                matchedRecipes = allRecipes;
            }

            return View(matchedRecipes);
        }
    }
}