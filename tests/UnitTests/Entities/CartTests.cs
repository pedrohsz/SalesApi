using SalesApi.Domain.Entities;

namespace SalesApi.Entities.Tests
{
    public class CartTests
    {
        [Fact]
        public void Should_Create_Cart_With_Valid_Data()
        {
            var cart = new Cart(Guid.NewGuid(), new List<CartItem> { new CartItem(Guid.NewGuid(), Guid.NewGuid(), 2) });

            Assert.NotNull(cart);
            Assert.NotEmpty(cart.Items);
        }

        [Fact]
        public void Should_Not_Create_Empty_Cart()
        {
            Assert.Throws<ArgumentException>(() => new Cart(Guid.NewGuid(), new List<CartItem>()));
        }

        [Fact]
        public void Should_Add_Item_To_Cart()
        {
            var cart = new Cart(Guid.NewGuid(), new List<CartItem>() { new CartItem(Guid.NewGuid(), Guid.NewGuid(), 1) });

            Assert.Single(cart.Items);
        }

        [Fact]
        public void Should_Remove_Item_From_Cart()
        {
            var productId = Guid.NewGuid();
            var cart = new Cart(Guid.NewGuid(), new List<CartItem> { new CartItem(Guid.NewGuid(), productId, 2) });

            cart.RemoveItem(productId);

            Assert.Empty(cart.Items);
        }

        [Fact]
        public void Should_Throw_Exception_When_Removing_Nonexistent_Item()
        {
            var cart = new Cart(Guid.NewGuid(), new List<CartItem>() { new CartItem(Guid.NewGuid(), Guid.NewGuid(), 1) });

            Assert.Throws<ArgumentException>(() => cart.RemoveItem(Guid.NewGuid()));
        }
    }
}
