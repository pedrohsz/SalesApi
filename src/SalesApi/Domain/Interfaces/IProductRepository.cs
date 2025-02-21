using SalesApi.Domain.Entities;

namespace SalesApi.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetPagedAsync(int page, int size);
        Task<(IEnumerable<Product>, int totalItems, int totalPages)> GetPagedAsync(int page, int size, string? orderBy);
        Task<Product> GetByIdAsync(Guid id);
        IQueryable<Product> GetAllQueryable();
        Task<Product> GetByTitleAsync(string title);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<string>> GetCategoriesAsync();
        Task<(IEnumerable<Product>, int totalItems, int totalPages)> GetProductsByCategoryAsync(string category, int page, int size, string? orderBy);
    }

}
