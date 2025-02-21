namespace SalesApi.Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid CartId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        public CartItem(Guid cartId, Guid productId, int quantity)
        {
            CartId = cartId != Guid.Empty ? cartId : throw new ArgumentException("CartId cannot be empty.");
            ProductId = productId;
            Quantity = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be at least 1.");
        }

        public void IncreaseQuantity(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Quantity increment must be positive.");

            Quantity += amount;
        }
    }
}