using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Application.Dtos;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Interfaces;
using WebApplication1.Infrastructure.Repositories;

namespace WebApplication1.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger, IMapper mapper)
        {
            _productRepository = productRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<Product>, int totalItems, int totalPages)> GetProductsPagedAsync(int page, int size, string? orderBy)
        {
            return await _productRepository.GetPagedAsync(page, size, orderBy);
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Product> CreateProductAsync(ProductDto productDto)
        {
            _logger.LogInformation("Creating a new product: {ProductTitle}", productDto.Title);

            if (productDto == null)
            {
                _logger.LogWarning("Product creation failed: Invalid data.");
                throw new ArgumentException("Product creation failed: Invalid data.");
            }

            // Verifica se já existe um produto com o mesmo título na base de dados
            var existingProduct = await _productRepository.GetByTitleAsync(productDto.Title);
            if (existingProduct != null)
            {
                _logger.LogWarning("Product creation failed: A product with title '{Title}' already exists.", productDto.Title);
                throw new InvalidOperationException($"A product with the title '{productDto.Title}' already exists.");
            }

            // Usa AutoMapper para converter ProductDto para Product
            var product = _mapper.Map<Product>(productDto);

            // Salva no banco de dados
            await _productRepository.AddAsync(product);

            _logger.LogInformation("Product created successfully with ID: {ProductId}", product.Id);
            return product;
        }

        public async Task<Product> UpdateProductAsync(Guid id, ProductDto productDto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;

            product = new Product(productDto.Title, productDto.Price, productDto.Description, productDto.Category, productDto.Image);
            await _productRepository.UpdateAsync(product);
            return product;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return false; // Produto não encontrado

            await _productRepository.DeleteAsync(id);
            return true; // Produto removido com sucesso
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            return await _productRepository.GetCategoriesAsync();
        }

        public async Task<(IEnumerable<Product>, int totalItems, int totalPages)> GetProductsByCategoryAsync(string category, int page, int size, string? orderBy)
        {
            return await _productRepository.GetProductsByCategoryAsync(category, page, size, orderBy);
        }

    }

}
