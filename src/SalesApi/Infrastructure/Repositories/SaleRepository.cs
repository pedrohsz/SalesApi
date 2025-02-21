using Microsoft.EntityFrameworkCore;
using SalesApi.Domain.Entities;
using SalesApi.Domain.Interfaces;
using SalesApi.Infrastructure.Data;

namespace SalesApi.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly SalesApiDbContext _context;

        public SaleRepository(SalesApiDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _context.Sales
                .Include(s => s.Items)
                .ToListAsync();
        }

        public async Task<Sale> GetByIdAsync(Guid id)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Sale sale)
        {
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
        }

        //public async Task AddAsync(Sale sale)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        _context.Sales.Add(sale);
        //        await _context.SaveChangesAsync(); // Salva a venda primeiro

        //        await transaction.CommitAsync(); // Confirma a transação
        //    }
        //    catch
        //    {
        //        await transaction.RollbackAsync(); // Reverte se algo falhar
        //        throw;
        //    }
        //}


        public async Task UpdateAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var sale = await GetByIdAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CancelItemAsync(Guid saleId, Guid productId)
        {
            var sale = await GetByIdAsync(saleId);
            if (sale != null)
            {
                var item = sale.Items.FirstOrDefault(i => i.ProductId == productId);
                if (item != null)
                {
                    sale.Items.Remove(item);
                    await UpdateAsync(sale);
                }
            }
        }
    }

}
