namespace SalesApi.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public DateTime Date { get; private set; } = DateTime.UtcNow;
        public List<CartItem> Products { get; private set; } = new();

        private Cart() { }

        public Cart(Guid userId, List<CartItem> items)
        {
            UserId = userId != Guid.Empty ? userId : throw new ArgumentException("UserId cannot be empty.");
            Products = items ?? throw new ArgumentException("Cart must have at least one item.");

            Validate();
        }

        private void Validate()
        {
            if (!Products.Any())
                throw new ArgumentException("Cart cannot be empty.");
        }

        public void AddItem(Guid productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be at least 1.");

            var existingItem = Products.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                Products.Add(new CartItem(Id, productId, quantity));
            }
        }

        public void RemoveItem(Guid productId)
        {
            var item = Products.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new ArgumentException("Item not found in the cart.");

            Products.Remove(item);
        }
    }
}
