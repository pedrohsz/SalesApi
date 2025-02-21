namespace SalesApi.Application.Dtos
{
    public class SaleItemDto
    {
        public Guid ProductId { get; set; } // ID do produto vendido
        public int Quantity { get; set; } // Quantidade vendida
        public decimal UnitPrice { get; set; } // Preço unitário do produto
        public Guid SaleId { get; set; }
    }
}
