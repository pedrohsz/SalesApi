namespace WebApplication1.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }
        public DateTime Date { get; private set; } = DateTime.UtcNow;
        public List<CartItem> Items { get; private set; } = new();

        private Cart() { }

        public Cart(Guid userId, List<CartItem> items)
        {
            UserId = userId;
            Items = items ?? new List<CartItem>();
            Validate();
        }

        private void Validate()
        {
            if (!Items.Any())
                throw new ArgumentException("Cart cannot be empty.");
        }
    }

}
