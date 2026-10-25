using FoodInventory.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodInventory.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

       
        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

                if (result == PasswordVerificationResult.Success)
                {
                    
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);
                    HttpContext.Session.SetInt32("UserId", user.Id);

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Nieprawidłowy login lub hasło";
            return View();
        }

        
        public IActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                ViewBag.Error = "Użytkownik o takiej nazwie już istnieje";
                return View();
            }

            var hasher = new PasswordHasher<User>();
            var newUser = new User
            {
                Username = username,
                Role = "User", 
                ApiToken = Guid.NewGuid().ToString() 
            };
            newUser.PasswordHash = hasher.HashPassword(newUser, password);

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

       
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}