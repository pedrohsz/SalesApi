using Microsoft.EntityFrameworkCore;
using SalesApi.Domain.Entities;
using SalesApi.Domain.Interfaces;
using SalesApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace SalesApi.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SalesApiDbContext _context;

        public ProductRepository(SalesApiDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetPagedAsync(int page, int size)
        {
            return await _context.Products
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        /// <summary>
        /// Obtém produtos paginados e ordenados
        /// </summary>
        public async Task<(IEnumerable<Product>, int totalItems, int totalPages)> GetPagedAsync(int page, int size, string? orderBy)
        {
            var query = GetAllQueryable(); // Obtém o IQueryable para modificar

            if (!string.IsNullOrEmpty(orderBy))
                query = ApplySorting(query, orderBy);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)size);

            var products = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return (products, totalItems, totalPages);
        }

        public IQueryable<Product> GetAllQueryable()
        {
            return _context.Products.AsNoTracking();
        }

        private static IQueryable<Product> ApplySorting(IQueryable<Product> query, string orderBy)
        {
            var orderParams = orderBy.Split(",");
            var propertyMappings = new Dictionary<string, Expression<Func<Product, object>>>
                {
                    { "title", p => p.Title },
                    { "price", p => p.Price },
                    { "description", p => p.Description },
                    { "category", p => p.Category }
                };

            foreach (var param in orderParams)
            {
                var trimmedParam = param.Trim();
                var descending = trimmedParam.EndsWith("desc", StringComparison.OrdinalIgnoreCase);
                var propertyName = trimmedParam.Split(" ")[0].ToLower();

                if (!propertyMappings.ContainsKey(propertyName))
                    continue;

                var propertyExpression = propertyMappings[propertyName];

                query = descending
                    ? query.OrderByDescending(propertyExpression)
                    : query.OrderBy(propertyExpression);
            }

            return query;
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            return await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();
        }

        public async Task<(IEnumerable<Product>, int totalItems, int totalPages)> GetProductsByCategoryAsync(string category, int page, int size, string? orderBy)
        {
            var query = _context.Products
                .Where(p => p.Category == category)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(orderBy))
                query = ApplySorting(query, orderBy);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)size);

            var products = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return (products, totalItems, totalPages);
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> GetByTitleAsync(string title)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Title == title);
        }

        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }

}
