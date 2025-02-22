namespace SalesApi.Domain.Entities
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

        public Sale() { }

        public Sale(string saleNumber, Guid customerId, Guid branchId, List<SaleItem> items)
        {
            SaleNumber = !string.IsNullOrWhiteSpace(saleNumber) ? saleNumber : throw new ArgumentException("Sale number cannot be empty.");
            CustomerId = customerId != Guid.Empty ? customerId : throw new ArgumentException("CustomerId cannot be empty.");
            BranchId = branchId != Guid.Empty ? branchId : throw new ArgumentException("BranchId cannot be empty.");
            Date = DateTime.UtcNow;
            Items = items ?? throw new ArgumentNullException(nameof(items));

            ValidateItems();
        }

        private void ValidateItems()
        {
            if (Items == null || !Items.Any())
                throw new ArgumentException("Sale must contain at least one item.");

            if (Items.Any(i => i.Quantity <= 0))
                throw new ArgumentException("Sale item quantity must be greater than zero.");

            if (Items.Any(i => i.UnitPrice < 0))
                throw new ArgumentException("Sale item unit price cannot be negative.");
        }

        public void CancelSale()
        {
            if (Cancelled)
                throw new InvalidOperationException("Sale is already cancelled.");

            Cancelled = true;

            // TODO: Emitir evento de domínio para integração com outros sistemas
        }

        public void CancelItem(Guid productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new ArgumentException("Item not found in the sale.");

            item.CancelItem();


            // Se todos os itens forem cancelados, cancela a venda automaticamente
            if (Items.All(i => i.IsCancelled == true))
                CancelSale();
        }
    }
}
