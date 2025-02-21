using SalesApi.Domain.Entities;

namespace SalesApi.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<Cart> GetByIdAsync(Guid id);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(Guid id);
    }

}
