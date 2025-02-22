namespace SalesApi.Application.Dtos
{
    public class SaleDto
    {
        public Guid? Id { get; set; }
        public string? SaleNumber { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public List<SaleItemDto> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
