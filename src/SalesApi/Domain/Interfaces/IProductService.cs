using SalesApi.Application.Dtos;
using SalesApi.Domain.Entities;

namespace SalesApi.Domain.Interfaces
{
    public interface IProductService
    {
        Task<(IEnumerable<Product>, int totalItems, int totalPages)> GetProductsPagedAsync(int page, int size, string? orderBy);
        Task<Product> GetProductByIdAsync(Guid id);
        Task<Product> CreateProductAsync(ProductDto productDto);
        Task<Product> UpdateProductAsync(Guid id, ProductDto productDto);
        Task<IEnumerable<string>> GetCategoriesAsync();
        Task<(IEnumerable<Product>, int totalItems, int totalPages)> GetProductsByCategoryAsync(string category, int page, int size, string? orderBy);

        Task<bool> DeleteProductAsync(Guid id);
    }

}
