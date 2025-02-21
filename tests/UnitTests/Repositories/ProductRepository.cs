using Microsoft.EntityFrameworkCore;
using SalesApi.Domain.Entities;
using SalesApi.Infrastructure.Data;
using SalesApi.Infrastructure.Repositories;

namespace SalesApi.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly DbContextOptions<SalesApiDbContext> _options;

        public ProductRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<SalesApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Product")
                .Options;
        }

        [Fact]
        public async Task Should_Add_Product_To_Database()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new ProductRepository(context);
                var product = new Product("Product A", 100.0m, "Description", "Category", "image.jpg");

                await repository.AddAsync(product);

                var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.Title == "Product A");
                Assert.NotNull(savedProduct);
            }
        }

        [Fact]
        public async Task Should_Get_Product_By_Id()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new ProductRepository(context);
                var productId = Guid.NewGuid();
                var product = new Product("Product B", 150.0m, "Description", "Category", "image.jpg");

                await repository.AddAsync(product);
                var retrievedProduct = await repository.GetByIdAsync(product.Id);

                Assert.NotNull(retrievedProduct);
                Assert.Equal(product.Id, retrievedProduct.Id);
            }
        }

        [Fact]
        public async Task Should_Get_Product_By_Title()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new ProductRepository(context);
                var product = new Product("Unique Product", 99.99m, "Special description", "Unique Category", "image.jpg");

                await repository.AddAsync(product);
                var retrievedProduct = await repository.GetByTitleAsync("Unique Product");

                Assert.NotNull(retrievedProduct);
                Assert.Equal("Unique Product", retrievedProduct.Title);
            }
        }

        [Fact]
        public async Task Should_Get_All_Products()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new ProductRepository(context);

                await repository.AddAsync(new Product("Product C", 50.0m, "Description", "Category", "image.jpg"));
                await repository.AddAsync(new Product("Product D", 75.0m, "Description", "Category", "image.jpg"));

                var allProducts = await repository.GetPagedAsync(1, 10);
                Assert.Equal(2, allProducts.Count());
            }
        }

        [Fact]
        public async Task Should_Get_Categories()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new ProductRepository(context);

                await repository.AddAsync(new Product("Product E", 100.0m, "Description", "Category1", "image.jpg"));
                await repository.AddAsync(new Product("Product F", 150.0m, "Description", "Category2", "image.jpg"));

                var categories = await repository.GetCategoriesAsync();
                Assert.Equal(2, categories.Count());
            }
        }

        [Fact]
        public async Task Should_Get_Products_By_Category()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new ProductRepository(context);

                await repository.AddAsync(new Product("Product G", 200.0m, "Description", "Category1", "image.jpg"));
                await repository.AddAsync(new Product("Product H", 300.0m, "Description", "Category1", "image.jpg"));
                await repository.AddAsync(new Product("Product I", 400.0m, "Description", "Category2", "image.jpg"));

                var (products, totalItems, totalPages) = await repository.GetProductsByCategoryAsync("Category1", 1, 10, null);

                Assert.Equal(2, products.Count());
                Assert.Equal(2, totalItems);
            }
        }

        [Fact]
        public async Task Should_Update_Product()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new ProductRepository(context);

                var product = new Product("Product J", 250.0m, "Old Description", "Category", "image.jpg");
                await repository.AddAsync(product);

                var existingProduct = await repository.GetByIdAsync(product.Id);

                existingProduct.UpdateProduct("Product J", 250.0m, "Updated Description", "Category", "image.jpg");

                await repository.UpdateAsync(existingProduct);

                var updatedProduct = await repository.GetByIdAsync(existingProduct.Id);
                Assert.Equal("Updated Description", updatedProduct.Description);
            }
        }

        [Fact]
        public async Task Should_Delete_Product()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new ProductRepository(context);
                var product = new Product("Product K", 300.0m, "Description", "Category", "image.jpg");

                await repository.AddAsync(product);
                await repository.DeleteAsync(product.Id);

                var deletedProduct = await repository.GetByIdAsync(product.Id);
                Assert.Null(deletedProduct);
            }
        }
    }
}
