using System.ComponentModel.DataAnnotations;

namespace FoodInventory.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Icon { get; set; } 
    }
}