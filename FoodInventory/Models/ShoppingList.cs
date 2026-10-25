    using System.ComponentModel.DataAnnotations;

namespace FoodInventory.Models
{
    public class ShoppingList
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        [Required]
        public double Quantity { get; set; }
        
        [Required]
        public string Unit { get; set; }
        
        public bool IsBought { get; set; } = false;
    }
}