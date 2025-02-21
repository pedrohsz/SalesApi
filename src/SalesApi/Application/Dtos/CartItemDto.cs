namespace SalesApi.Application.Dtos
{
    public class CartItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public Guid CartId { get; private set; }

    }
}
