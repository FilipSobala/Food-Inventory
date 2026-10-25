using System.ComponentModel.DataAnnotations;

namespace FoodInventory.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
        public int DefaultExpiryDays { get; set; }
    }
}