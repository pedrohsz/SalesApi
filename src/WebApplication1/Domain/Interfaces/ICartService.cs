using WebApplication1.Application.Dtos;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetCartsAsync();
        Task<Cart> GetCartByIdAsync(Guid id);
        Task<Cart> CreateCartAsync(CartDto cartDto);
        Task DeleteCartAsync(Guid id);
    }

}
