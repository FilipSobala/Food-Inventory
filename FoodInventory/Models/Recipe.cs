using System.ComponentModel.DataAnnotations;

namespace FoodInventory.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Instructions { get; set; }
        
        public int PrepTimeMinutes { get; set; }
        
        public List<RecipeProduct> RecipeProducts { get; set; } = new List<RecipeProduct>();
    }
}