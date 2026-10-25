using System.ComponentModel.DataAnnotations;

namespace FoodInventory.Models
{
    public class RecipeProduct
    {
        [Key]
        public int Id { get; set; } // Dodajemy prosty Id dla ułatwienia operacji w EF

        [Required]
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        [Required]
        public double Quantity { get; set; }
        
        [Required]
        public string Unit { get; set; }
    }
}