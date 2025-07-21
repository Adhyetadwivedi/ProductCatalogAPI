using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    public class Product
    {
        [Key]
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StockAvailable { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
