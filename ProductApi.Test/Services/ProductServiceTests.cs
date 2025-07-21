using Microsoft.Extensions.Logging;
using Moq;
using ProductApi.DTOs;
using ProductApi.Interfaces;
using ProductApi.Models;
using ProductApi.Services;
using Xunit;

namespace ProductApi.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly Mock<IProductIdGenerator> _mockIdGenerator;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _mockIdGenerator = new Mock<IProductIdGenerator>();
            _mockLogger = new Mock<ILogger<ProductService>>();
            _service = new ProductService(_mockRepo.Object, _mockIdGenerator.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidProduct_ReturnsProduct()
        {
            // Arrange
            var dto = new ProductDto
            {
                Name = "Test Product",
                Description = "Test Description",
                StockAvailable = 10,
                Price = 99.99m
            };

            _mockIdGenerator.Setup(x => x.GenerateAsync()).ReturnsAsync("123456");
            _mockRepo.Setup(x => x.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123456", result.ProductId);
            Assert.Equal("Test Product", result.Name);
            Assert.Equal("Test Description", result.Description);
            Assert.Equal(10, result.StockAvailable);
            Assert.Equal(99.99m, result.Price);
            _mockRepo.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingProduct_ReturnsProduct()
        {
            // Arrange
            var product = new Product
            {
                ProductId = "123456",
                Name = "Test Product",
                Description = "Test Description",
                StockAvailable = 10,
                Price = 99.99m
            };

            _mockRepo.Setup(x => x.GetByIdAsync("123456")).ReturnsAsync(product);

            // Act
            var result = await _service.GetByIdAsync("123456");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123456", result.ProductId);
            Assert.Equal("Test Product", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingProduct_ReturnsNull()
        {
            // Arrange
            _mockRepo.Setup(x => x.GetByIdAsync("999999")).ReturnsAsync((Product?)null);

            // Act
            var result = await _service.GetByIdAsync("999999");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ExistingProduct_ReturnsUpdatedProduct()
        {
            // Arrange
            var existingProduct = new Product
            {
                ProductId = "123456",
                Name = "Old Name",
                Description = "Old Description",
                StockAvailable = 5,
                Price = 50.00m
            };

            var updateDto = new ProductDto
            {
                Name = "Updated Name",
                Description = "Updated Description",
                StockAvailable = 15,
                Price = 75.00m
            };

            _mockRepo.Setup(x => x.GetByIdAsync("123456")).ReturnsAsync(existingProduct);
            _mockRepo.Setup(x => x.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync("123456", updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Name", result.Name);
            Assert.Equal("Updated Description", result.Description);
            Assert.Equal(15, result.StockAvailable);
            Assert.Equal(75.00m, result.Price);
            _mockRepo.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingProduct_ReturnsNull()
        {
            // Arrange
            var updateDto = new ProductDto { Name = "Test" };
            _mockRepo.Setup(x => x.GetByIdAsync("999999")).ReturnsAsync((Product?)null);

            // Act
            var result = await _service.UpdateAsync("999999", updateDto);

            // Assert
            Assert.Null(result);
            _mockRepo.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ExistingProduct_ReturnsTrue()
        {
            // Arrange
            var product = new Product { ProductId = "123456", Name = "Test Product" };
            _mockRepo.Setup(x => x.GetByIdAsync("123456")).ReturnsAsync(product);
            _mockRepo.Setup(x => x.DeleteAsync(product)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAsync("123456");

            // Assert
            Assert.True(result);
            _mockRepo.Verify(x => x.DeleteAsync(product), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingProduct_ReturnsFalse()
        {
            // Arrange
            _mockRepo.Setup(x => x.GetByIdAsync("999999")).ReturnsAsync((Product?)null);

            // Act
            var result = await _service.DeleteAsync("999999");

            // Assert
            Assert.False(result);
            _mockRepo.Verify(x => x.DeleteAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task DecrementStockAsync_SufficientStock_ReturnsTrue()
        {
            // Arrange
            var product = new Product
            {
                ProductId = "123456",
                Name = "Test Product",
                StockAvailable = 10
            };

            _mockRepo.Setup(x => x.GetByIdAsync("123456")).ReturnsAsync(product);
            _mockRepo.Setup(x => x.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DecrementStockAsync("123456", 5);

            // Assert
            Assert.True(result);
            Assert.Equal(5, product.StockAvailable);
            _mockRepo.Verify(x => x.UpdateAsync(product), Times.Once);
        }

        [Fact]
        public async Task DecrementStockAsync_InsufficientStock_ReturnsFalse()
        {
            // Arrange
            var product = new Product
            {
                ProductId = "123456",
                Name = "Test Product",
                StockAvailable = 3
            };

            _mockRepo.Setup(x => x.GetByIdAsync("123456")).ReturnsAsync(product);

            // Act
            var result = await _service.DecrementStockAsync("123456", 5);

            // Assert
            Assert.False(result);
            Assert.Equal(3, product.StockAvailable); // Stock should remain unchanged
            _mockRepo.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task AddStockAsync_ExistingProduct_ReturnsTrue()
        {
            // Arrange
            var product = new Product
            {
                ProductId = "123456",
                Name = "Test Product",
                StockAvailable = 10
            };

            _mockRepo.Setup(x => x.GetByIdAsync("123456")).ReturnsAsync(product);
            _mockRepo.Setup(x => x.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddStockAsync("123456", 5);

            // Assert
            Assert.True(result);
            Assert.Equal(15, product.StockAvailable);
            _mockRepo.Verify(x => x.UpdateAsync(product), Times.Once);
        }

        [Fact]
        public async Task AddStockAsync_NonExistingProduct_ReturnsFalse()
        {
            // Arrange
            _mockRepo.Setup(x => x.GetByIdAsync("999999")).ReturnsAsync((Product?)null);

            // Act
            var result = await _service.AddStockAsync("999999", 5);

            // Assert
            Assert.False(result);
            _mockRepo.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }
    }
}