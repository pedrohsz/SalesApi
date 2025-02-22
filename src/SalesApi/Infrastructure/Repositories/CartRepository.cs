using Microsoft.EntityFrameworkCore;
using SalesApi.Domain.Entities;
using SalesApi.Domain.Interfaces;
using SalesApi.Infrastructure.Data;

namespace SalesApi.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly SalesApiDbContext _context;

        public CartRepository(SalesApiDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _context.Carts.Include(c => c.Products).ToListAsync();
        }

        public async Task<Cart> GetByIdAsync(Guid id)
        {
            return await _context.Carts.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var cart = await GetByIdAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }
    }

}
