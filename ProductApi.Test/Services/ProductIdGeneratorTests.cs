using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Models;
using ProductApi.Services;
using Xunit;

namespace ProductApi.Tests.Services
{
    public class ProductIdGeneratorTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task GenerateAsync_FirstTime_ReturnsValidId()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var generator = new ProductIdGenerator(context);

            // Act
            var id = await generator.GenerateAsync();

            // Assert
            Assert.NotNull(id);
            Assert.Equal(6, id.Length);
            Assert.True(int.TryParse(id, out int numericId));
            Assert.Equal(100001, numericId); // First ID should be 100001
        }

        [Fact]
        public async Task GenerateAsync_MultipleCalls_ReturnsSequentialIds()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var generator = new ProductIdGenerator(context);

            // Act
            var id1 = await generator.GenerateAsync();
            var id2 = await generator.GenerateAsync();
            var id3 = await generator.GenerateAsync();

            // Assert
            Assert.Equal("100001", id1);
            Assert.Equal("100002", id2);
            Assert.Equal("100003", id3);
        }

        [Fact]
        public async Task GenerateAsync_ConcurrentCalls_GeneratesUniqueIds()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var generator = new ProductIdGenerator(context);

            // Act
            var tasks = Enumerable.Range(0, 10)
                .Select(_ => generator.GenerateAsync())
                .ToArray();

            var ids = await Task.WhenAll(tasks);

            // Assert
            Assert.Equal(10, ids.Length);
            Assert.Equal(10, ids.Distinct().Count()); // All IDs should be unique
        }
    }
}