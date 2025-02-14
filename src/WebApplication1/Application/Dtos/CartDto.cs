namespace WebApplication1.Application.Dtos
{
    public class CartDto
    {
        public Guid UserId { get; set; }
        public List<CartItemDto> Products { get; set; }
    }
}
