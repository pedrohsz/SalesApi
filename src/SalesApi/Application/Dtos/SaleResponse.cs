using SalesApi.Domain.Entities;

namespace SalesApi.Application.Dtos
{
    public class SaleResponse
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime Date { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public decimal TotalAmount => Items.Sum(i => i.Total);
        public bool Cancelled { get; set; }
        public List<SaleItem> Items { get; set; } = new();
    }

    public class SaleItemResponse
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total => (UnitPrice * Quantity) - Discount;
        public bool IsCancelled { get; set; }
    }
}
