namespace WebApplication1.Domain.Entities
{
    public class CartItem
    {
        //public Guid Id { get; private set; }
        public Guid CartId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        public CartItem(Guid cartId, Guid productId, int quantity)
        {
            //Id = Guid.NewGuid();
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            Validate();
        }

        private void Validate()
        {
            if (Quantity < 1)
                throw new ArgumentException("Quantity must be at least 1.");
        }
    }

}
