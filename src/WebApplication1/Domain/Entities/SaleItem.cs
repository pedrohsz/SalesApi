namespace WebApplication1.Domain.Entities
{
    public class SaleItem
    {
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Total => (UnitPrice * Quantity) - Discount;
        public Guid SaleId { get; private set; }
        public string ProductName { get; private set; }

        public SaleItem() { }

        public SaleItem(Guid saleId, Guid productId, string productName, int quantity, decimal unitPrice)
        {
            SaleId = saleId;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = CalculateDiscount();
            Validate();
        }

        private decimal CalculateDiscount()
        {
            decimal total = UnitPrice * Quantity;

            if (Quantity >= 10 && Quantity <= 20) return total * 0.20m;
            if (Quantity >= 4) return total * 0.10m; 
            return 0;
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(ProductName))
                throw new ArgumentException("Product name cannot be empty.");

            if (Quantity < 1 || Quantity > 20)
                throw new ArgumentException("Quantity must be between 1 and 20.");
        }

        public void CancelItem() // verificar
        {
            Quantity = 0;
            Discount = 0;
        }
    }

}
