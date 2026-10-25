using FoodInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FoodInventory.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsApiController(AppDbContext context)
        {
            _context = context;
        }

    
        private User ValidateApiRequest()
        {
            if (!Request.Headers.TryGetValue("X-Username", out var username) ||
                !Request.Headers.TryGetValue("X-Api-Token", out var token))
            {
                return null;
            }

            return _context.Users.FirstOrDefault(u => u.Username == username.ToString() && u.ApiToken == token.ToString());
        }

        
        [HttpGet]
        public IActionResult GetMyProducts()
        {
            var user = ValidateApiRequest();
            if (user == null) return Unauthorized("Błędne dane autoryzacji API (Username lub Token).");

            var products = _context.UserProducts
                .Include(up => up.Product)
                .Where(up => up.UserId == user.Id)
                .Select(up => new { up.Id, ProductName = up.Product.Name, up.Quantity, up.Unit, up.ExpiryDate, up.Notes })
                .ToList();

            return Ok(products);
        }

       
        [HttpPost]
        public IActionResult AddProduct([FromBody] ApiProductModel model)
        {
            var user = ValidateApiRequest();
            if (user == null) return Unauthorized("Błędne dane autoryzacji API.");

            var baseProduct = _context.Products.FirstOrDefault(p => p.Id == model.ProductId);
            if (baseProduct == null) return BadRequest("Nie znaleziono produktu bazowego o podanym ID.");

            var newUserProduct = new UserProduct
            {
                UserId = user.Id,
                ProductId = model.ProductId,
                Quantity = model.Quantity,
                Unit = model.Unit,
                ExpiryDate = model.ExpiryDate,
                Notes = model.Notes
            };

            _context.UserProducts.Add(newUserProduct);
            _context.SaveChanges();

            return Ok(new { Message = "Produkt został dodany przez REST API!", ProductId = newUserProduct.Id });
        }

        
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var user = ValidateApiRequest();
            if (user == null) return Unauthorized("Błędne dane autoryzacji API.");

            var product = _context.UserProducts.FirstOrDefault(up => up.Id == id && up.UserId == user.Id);
            if (product == null) return NotFound("Nie znaleziono produktu o tym ID w Twojej lodówce.");

            _context.UserProducts.Remove(product);
            _context.SaveChanges();

            return Ok(new { Message = "Produkt został usunięty pomyślnie przez REST API!" });
        }
    }

  
    public class ApiProductModel
    {
        public int ProductId { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Notes { get; set; }
    }
}