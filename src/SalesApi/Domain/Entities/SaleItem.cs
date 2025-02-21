namespace SalesApi.Domain.Entities
{
    public class SaleItem
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Total => (UnitPrice * Quantity) - Discount;
        public Guid SaleId { get; private set; }
        public bool IsCancelled { get; private set; } 

        public SaleItem() { }

        public SaleItem(Guid saleId, Guid productId, int quantity, decimal unitPrice)
        {
            SaleId = saleId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            IsCancelled = false;

            Quantity = ValidateQuantity(quantity);
            Discount = CalculateDiscount();
            Validate();
        }

        public void CancelItem()
        {
            if (IsCancelled)
                throw new InvalidOperationException("Item is already cancelled.");

            IsCancelled = true;
        }

        private int ValidateQuantity(int quantity)
        {
            if (quantity < 1)
                throw new ArgumentException("Quantity must be at least 1.");
            if (quantity > 20)
                throw new ArgumentException("Cannot sell more than 20 identical items.");

            return quantity;
        }

        private decimal CalculateDiscount()
        {
            if (Quantity < 4)
                return 0;

            if (Quantity >= 4 && Quantity < 10)
                return (UnitPrice * Quantity) * 0.10m;

            if (Quantity >= 10 && Quantity <= 20)
                return (UnitPrice * Quantity) * 0.20m;

            return 0;
        }

        private void Validate()
        {
            if (Quantity < 1)
                throw new ArgumentException("Quantity must be at least 1.");

            if (UnitPrice < 0)
                throw new ArgumentException("UnitPrice cannot be negative.");
        }
    }
}
