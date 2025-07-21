using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    public class ProductIdTracker
    {
        [Key]
        public int Id { get; set; }
        public int LastGeneratedId { get; set; }
    }
}