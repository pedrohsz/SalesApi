using SalesApi.Domain.Entities;

namespace SalesApi.Entities.Tests
{
    public class SaleTests
    {
        [Fact]
        public void Should_Create_Sale_With_Valid_Data()
        {
            var items = new List<SaleItem>
            {
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 10.0m), // 45 reais
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 10, 20.0m) // 160 + 45 = 205
            };

            var sale = new Sale("SALE123", Guid.NewGuid(), Guid.NewGuid(), items);

            Assert.NotNull(sale);
            Assert.Equal("SALE123", sale.SaleNumber);
            Assert.False(sale.Cancelled);
            Assert.Equal(205m, sale.TotalAmount);
        }

        [Fact]
        public void Should_Apply_Discount_Correctly()
        {
            var items = new List<SaleItem>
            {
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 10.0m), // 10% de desconto
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 15, 20.0m) // 20% de desconto
            };

            var sale = new Sale("SALE124", Guid.NewGuid(), Guid.NewGuid(), items);

            Assert.Equal(285m, sale.TotalAmount); // Verifica o desconto corretamente
        }

        [Fact]
        public void Should_Cancel_Sale_When_All_Items_Are_Cancelled()
        {
            var items = new List<SaleItem>
            {
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 10.0m),
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 2, 20.0m)
            };

            var sale = new Sale("SALE125", Guid.NewGuid(), Guid.NewGuid(), items);

            foreach (var item in sale.Items)
            {
                sale.CancelItem(item.ProductId);
            }

            Assert.True(sale.Cancelled);
        }

        [Fact]
        public void Should_Not_Cancel_Sale_If_At_Least_One_Item_Is_Active()
        {
            var items = new List<SaleItem>
            {
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 10.0m),
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 2, 20.0m)
            };

            var sale = new Sale("SALE126", Guid.NewGuid(), Guid.NewGuid(), items);

            sale.CancelItem(items[0].ProductId);

            Assert.False(sale.Cancelled);
        }

        [Fact]
        public void Should_Throw_Exception_When_Cancelling_Nonexistent_Item()
        {
            var saleItem = new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 10.0m);
            var sale = new Sale("SALE127", Guid.NewGuid(), Guid.NewGuid(), new List<SaleItem>() { saleItem });

            Assert.Throws<ArgumentException>(() => sale.CancelItem(Guid.NewGuid()));
        }

        [Fact]
        public void Should_Throw_Exception_When_Creating_Sale_Without_Items()
        {
            Assert.Throws<ArgumentException>(() => new Sale("SALE128", Guid.NewGuid(), Guid.NewGuid(), new List<SaleItem>()));
        }

        [Fact]
        public void Should_Throw_Exception_When_Creating_Sale_With_Empty_SaleNumber()
        {
            var items = new List<SaleItem>
            {
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 10.0m)
            };

            Assert.Throws<ArgumentException>(() => new Sale("", Guid.NewGuid(), Guid.NewGuid(), items));
        }

        [Fact]
        public void Should_Throw_Exception_When_Creating_Sale_With_Invalid_CustomerId()
        {
            var items = new List<SaleItem>
            {
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 10.0m)
            };

            Assert.Throws<ArgumentException>(() => new Sale("SALE129", Guid.Empty, Guid.NewGuid(), items));
        }

        [Fact]
        public void Should_Throw_Exception_When_Creating_Sale_With_Invalid_BranchId()
        {
            var items = new List<SaleItem>
            {
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 10.0m)
            };

            Assert.Throws<ArgumentException>(() => new Sale("SALE130", Guid.NewGuid(), Guid.Empty, items));
        }
    }
}
