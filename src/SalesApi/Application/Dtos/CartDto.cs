namespace SalesApi.Application.Dtos
{
    public class CartDto
    {
        public Guid UserId { get; set; }
        public List<CartItemDto> Items { get; set; }
    }
}
