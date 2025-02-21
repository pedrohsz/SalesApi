using SalesApi.Domain.Entities;

namespace SalesApi.Entities.Tests
{
    public class SaleItemTests
    {
        [Fact]
        public void Should_Create_SaleItem_With_Valid_Data()
        {
            var saleItem = new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 10.0m);

            Assert.NotNull(saleItem);
            Assert.Equal(5, saleItem.Quantity);
            Assert.False(saleItem.IsCancelled);
            Assert.Equal(45m, saleItem.Total);
        }

        [Fact]
        public void Should_Apply_Discount_Correctly()
        {
            var saleItem = new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 10, 20.0m);

            Assert.Equal(160m, saleItem.Total); // Aplicado 20% de desconto para 10 unidades
        }

        [Fact]
        public void Should_Cancel_SaleItem()
        {
            var saleItem = new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 10.0m);

            saleItem.CancelItem();

            Assert.True(saleItem.IsCancelled);
        }

        [Fact]
        public void Should_Not_Cancel_SaleItem_Twice()
        {
            var saleItem = new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 10.0m);

            saleItem.CancelItem();

            Assert.Throws<InvalidOperationException>(() => saleItem.CancelItem());
        }

        [Fact]
        public void Should_Throw_Exception_When_Creating_Invalid_Quantity()
        {
            Assert.Throws<ArgumentException>(() => new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 0, 10.0m));
        }

        [Fact]
        public void Should_Throw_Exception_When_Creating_Invalid_Price()
        {
            Assert.Throws<ArgumentException>(() => new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, -10.0m));
        }
    }
}
