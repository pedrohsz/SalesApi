using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<Cart> GetByIdAsync(Guid id);
        Task AddAsync(Cart cart);
        Task DeleteAsync(Guid id);
    }

}
