using SalesApi.Domain.Entities;

namespace SalesApi.Entities.Tests
{
    public class ProductTests
    {
        [Fact]
        public void Should_Create_Product_With_Valid_Data()
        {
            var product = new Product("Product A", 100.0m, "Description", "Category", "image.jpg");

            Assert.NotNull(product);
            Assert.Equal("Product A", product.Title);
            Assert.Equal(100.0m, product.Price);
        }

        [Fact]
        public void Should_Throw_Exception_When_Creating_Invalid_Price()
        {
            Assert.Throws<ArgumentException>(() => new Product("Product A", -100.0m, "Description", "Category", "image.jpg"));
        }

        [Fact]
        public void Should_Throw_Exception_When_Creating_Empty_Title()
        {
            Assert.Throws<ArgumentException>(() => new Product("", 100.0m, "Description", "Category", "image.jpg"));
        }
    }
}
