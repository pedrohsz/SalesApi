using SalesApi.Application.Dtos;
using SalesApi.Domain.Entities;

namespace SalesApi.Domain.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetCartsAsync();
        Task<Cart> GetCartByIdAsync(Guid id);
        Task<Cart> CreateCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task DeleteCartAsync(Guid id);
    }

}
