using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApi.Controllers;
using ProductApi.DTOs;
using ProductApi.Interfaces;
using ProductApi.Models;
using Xunit;

namespace ProductApi.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductsController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { ProductId = "123456", Name = "Product 1", StockAvailable = 10 },
                new Product { ProductId = "123457", Name = "Product 2", StockAvailable = 5 }
            };

            _mockService.Setup(x => x.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal(2, returnedProducts.Count());
        }

        [Fact]
        public async Task Get_ExistingProduct_ReturnsOk()
        {
            // Arrange
            var product = new Product { ProductId = "123456", Name = "Test Product" };
            _mockService.Setup(x => x.GetByIdAsync("123456")).ReturnsAsync(product);

            // Act
            var result = await _controller.Get("123456");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal("123456", returnedProduct.ProductId);
        }

        [Fact]
        public async Task Get_NonExistingProduct_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(x => x.GetByIdAsync("999999")).ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.Get("999999");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ValidProduct_ReturnsCreatedAtAction()
        {
            // Arrange
            var dto = new ProductDto { Name = "New Product", StockAvailable = 10, Price = 99.99m };
            var createdProduct = new Product { ProductId = "123456", Name = "New Product" };

            _mockService.Setup(x => x.CreateAsync(dto)).ReturnsAsync(createdProduct);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Get", createdResult.ActionName);
            Assert.Equal("123456", ((Product)createdResult.Value).ProductId);
        }

        [Fact]
        public async Task Update_ExistingProduct_ReturnsOk()
        {
            // Arrange
            var dto = new ProductDto { Name = "Updated Product" };
            var updatedProduct = new Product { ProductId = "123456", Name = "Updated Product" };

            _mockService.Setup(x => x.UpdateAsync("123456", dto)).ReturnsAsync(updatedProduct);

            // Act
            var result = await _controller.Update("123456", dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal("Updated Product", returnedProduct.Name);
        }

        [Fact]
        public async Task Delete_ExistingProduct_ReturnsOk()
        {
            // Arrange
            _mockService.Setup(x => x.DeleteAsync("123456")).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete("123456");

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DecrementStock_Success_ReturnsOk()
        {
            // Arrange
            _mockService.Setup(x => x.DecrementStockAsync("123456", 5)).ReturnsAsync(true);

            // Act
            var result = await _controller.DecrementStock("123456", 5);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DecrementStock_Failure_ReturnsBadRequest()
        {
            // Arrange
            _mockService.Setup(x => x.DecrementStockAsync("123456", 50)).ReturnsAsync(false);

            // Act
            var result = await _controller.DecrementStock("123456", 50);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}