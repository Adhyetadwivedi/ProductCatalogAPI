using ProductApi.DTOs;
using ProductApi.Interfaces;
using ProductApi.Models;

namespace ProductApi.Services
{



    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IProductIdGenerator _idGenerator;

        public ProductService(IProductRepository repo, IProductIdGenerator idGenerator)
        {
            _repo = repo;
            _idGenerator = idGenerator;
        }

        public async Task<IEnumerable<Product>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Product?> GetByIdAsync(string id) => await _repo.GetByIdAsync(id);

        public async Task<Product> CreateAsync(ProductDto dto)
        {
            var product = new Product
            {
                ProductId = await _idGenerator.GenerateAsync(),
                Name = dto.Name,
                Description = dto.Description,
                StockAvailable = dto.StockAvailable,
                Price = dto.Price
            };

            await _repo.AddAsync(product);
            return product;
        }

        public async Task<Product?> UpdateAsync(string id, ProductDto dto)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return null;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.StockAvailable = dto.StockAvailable;
            product.Price = dto.Price;

            await _repo.UpdateAsync(product);
            return product;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return false;

            await _repo.DeleteAsync(product);
            return true;
        }

        public async Task<bool> DecrementStockAsync(string id, int quantity)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null || product.StockAvailable < quantity) return false;

            product.StockAvailable -= quantity;
            await _repo.UpdateAsync(product);
            return true;
        }

        public async Task<bool> AddStockAsync(string id, int quantity)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return false;

            product.StockAvailable += quantity;
            await _repo.UpdateAsync(product);
            return true;
        }
    }
}