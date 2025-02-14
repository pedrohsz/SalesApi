namespace WebApplication1.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string SaleNumber { get; private set; }
        public DateTime Date { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid BranchId { get; private set; }
        public decimal TotalAmount => Items.Sum(i => i.Total);
        public bool Cancelled { get; private set; }
        public List<SaleItem> Items { get; private set; } = new();
        public DateTime CreatedAt { get; private set; }

        public Sale() { }

        public Sale(string saleNumber, Guid customerId, Guid branchId, List<SaleItem> items)
        {
            SaleNumber = saleNumber;
            Date = DateTime.UtcNow;
            CustomerId = customerId;
            BranchId = branchId;
            Items = items ?? throw new ArgumentNullException(nameof(items));
            ApplyDiscounts();
            Validate();
        }

        public void CancelSale()
        {
            if (Cancelled)
                throw new InvalidOperationException("Sale is already cancelled.");

            Cancelled = true;

            foreach (var item in Items)
            {
                item.CancelItem();
            }
        }

        public void CancelItem(Guid productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new ArgumentException("Item not found in the sale.");

            item.CancelItem();

            // Se todos os itens forem cancelados, cancela a venda
            if (Items.All(i => i.Quantity == 0))
            {
                CancelSale();
            }
        }

        private void ApplyDiscounts()
        {
            // Recria os itens aplicando os descontos
            Items = Items.Select(item =>
                new SaleItem(item.SaleId, item.ProductId, item.ProductName, item.Quantity, item.UnitPrice)).ToList();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(SaleNumber))
                throw new ArgumentException("Sale number cannot be empty.");

            if (Items == null || !Items.Any())
                throw new ArgumentException("Sale must contain at least one item.");
        }
    }

}
