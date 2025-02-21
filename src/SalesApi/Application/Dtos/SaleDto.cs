namespace SalesApi.Application.Dtos
{
    public class SaleDto
    {
        public Guid? Id { get; set; } // Identificador da venda (para buscas ou cancelamentos)
        public string? SaleNumber { get; set; } // Número único da venda
        public Guid CustomerId { get; set; } // Cliente da venda
        public Guid BranchId { get; set; } // Filial onde foi feita a venda
        public List<SaleItemDto> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
