namespace ProductApi.DTOs
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int StockAvailable { get; set; }
        public decimal Price { get; set; }
    }
}