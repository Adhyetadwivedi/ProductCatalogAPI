using ProductApi.DTOs;
using ProductApi.Models;

namespace ProductApi.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(string id);
        Task<Product> CreateAsync(ProductDto dto);
        Task<Product?> UpdateAsync(string id, ProductDto dto);
        Task<bool> DeleteAsync(string id);
        Task<bool> DecrementStockAsync(string id, int quantity);
        Task<bool> AddStockAsync(string id, int quantity);
    }
}
