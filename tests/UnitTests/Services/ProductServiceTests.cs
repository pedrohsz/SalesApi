using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WebApplication1.Application.Dtos;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services;
using WebApplication1.Infrastructure.Repositories;
namespace UnitTests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProductService _productService;
        private readonly Faker _faker;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<ProductService>>();
            _mockMapper = new Mock<IMapper>();

            _productService = new ProductService(
                _mockRepository.Object,
                _mockLogger.Object,
                _mockMapper.Object
            );

            _faker = new Faker("pt_BR");
        }

        /// <summary>
        /// Testa a criação de um produto garantindo que os dados sejam únicos em cada execução.
        /// </summary>
        [Fact]
        public async Task CreateProductAsync_ShouldCreateProductWithUniqueData()
        {
            var productDto = GenerateFakeProductDto();
            var product = new Product(productDto.Title, productDto.Price, productDto.Description, productDto.Category, productDto.Image);

            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<ProductDto>())).Returns(product);

            _mockRepository.Setup(repo => repo.GetByTitleAsync(productDto.Title))
                           .ReturnsAsync((Product)null);

            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var createdProduct = await _productService.CreateProductAsync(productDto);

            createdProduct.Should().NotBeNull();
            createdProduct.Title.Should().Be(productDto.Title);
            createdProduct.Price.Should().Be(productDto.Price);
            createdProduct.Description.Should().Be(productDto.Description);
            createdProduct.Category.Should().Be(productDto.Category);
            createdProduct.Image.Should().Be(productDto.Image);

            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        /// <summary>
        /// Testa a recuperação de um produto por ID.
        /// </summary>
        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            var product = GenerateFakeProduct();

            _mockRepository.Setup(repo => repo.GetByIdAsync(product.Id)).ReturnsAsync(product);

            var retrievedProduct = await _productService.GetProductByIdAsync(product.Id);

            retrievedProduct.Should().NotBeNull();
            retrievedProduct.Id.Should().Be(product.Id);
            retrievedProduct.Title.Should().Be(product.Title);
        }

        /// <summary>
        /// Testa a tentativa de recuperar um produto que não existe.
        /// </summary>
        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product)null);

            var retrievedProduct = await _productService.GetProductByIdAsync(Guid.NewGuid());

            retrievedProduct.Should().BeNull();
        }

        /// <summary>
        /// Testa a atualização de um produto.
        /// </summary>
        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateExistingProduct()
        {
            var existingProduct = GenerateFakeProduct();
            var updatedProductDto = GenerateFakeProductDto();

            _mockRepository.Setup(repo => repo.GetByIdAsync(existingProduct.Id)).ReturnsAsync(existingProduct);
            _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var updatedProduct = await _productService.UpdateProductAsync(existingProduct.Id, updatedProductDto);

            updatedProduct.Should().NotBeNull();
            updatedProduct.Title.Should().Be(updatedProductDto.Title);
            updatedProduct.Price.Should().Be(updatedProductDto.Price);
            updatedProduct.Description.Should().Be(updatedProductDto.Description);
            updatedProduct.Category.Should().Be(updatedProductDto.Category);
            updatedProduct.Image.Should().Be(updatedProductDto.Image);

            _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        /// <summary>
        /// Testa a remoção de um produto existente.
        /// </summary>
        [Fact]
        public async Task DeleteProductAsync_ShouldReturnTrue_WhenProductExists()
        {
            var product = GenerateFakeProduct();

            _mockRepository.Setup(repo => repo.GetByIdAsync(product.Id)).ReturnsAsync(product);
            _mockRepository.Setup(repo => repo.DeleteAsync(product.Id)).Returns(Task.CompletedTask);

            var result = await _productService.DeleteProductAsync(product.Id);

            result.Should().BeTrue();
            _mockRepository.Verify(repo => repo.DeleteAsync(product.Id), Times.Once);
        }

        /// <summary>
        /// Testa a remoção de um produto que não existe.
        /// </summary>
        [Fact]
        public async Task DeleteProductAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product)null);

            var result = await _productService.DeleteProductAsync(Guid.NewGuid());

            result.Should().BeFalse();
        }

        /// <summary>
        /// Testa a obtenção de todas as categorias de produtos.
        /// </summary>
        [Fact]
        public async Task GetCategoriesAsync_ShouldReturnListOfCategories()
        {
            var categories = new List<string> { "Eletrônicos", "Livros", "Roupas" };

            _mockRepository.Setup(repo => repo.GetCategoriesAsync()).ReturnsAsync(categories);

            var result = await _productService.GetCategoriesAsync();

            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(categories);
        }

        /// <summary>
        /// Testa a busca de produtos paginados por categoria.
        /// </summary>
        [Fact]
        public async Task GetProductsByCategoryAsync_ShouldReturnPagedProducts()
        {
            var category = "Eletrônicos";
            var page = 1;
            var size = 10;
            var totalItems = 15;
            var totalPages = 2;
            var products = new List<Product> { GenerateFakeProduct(), GenerateFakeProduct() };

            _mockRepository.Setup(repo => repo.GetProductsByCategoryAsync(category, page, size, null))
                           .ReturnsAsync((products, totalItems, totalPages));

            var (resultProducts, resultTotalItems, resultTotalPages) =
                await _productService.GetProductsByCategoryAsync(category, page, size, null);

            resultProducts.Should().NotBeNullOrEmpty();
            resultTotalItems.Should().Be(totalItems);
            resultTotalPages.Should().Be(totalPages);
        }

        /// <summary>
        /// Gera um ProductDto com dados aleatórios.
        /// </summary>
        private ProductDto GenerateFakeProductDto() => new Faker<ProductDto>()
            .RuleFor(p => p.Title, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
            .RuleFor(p => p.Image, f => f.Image.PicsumUrl());

        /// <summary>
        /// Gera um Product com dados aleatórios.
        /// </summary>
        private Product GenerateFakeProduct() => new Product(
            _faker.Commerce.ProductName(),
            _faker.Random.Decimal(10, 1000),
            _faker.Commerce.ProductDescription(),
            _faker.Commerce.Categories(1)[0],
            _faker.Image.PicsumUrl()
        );
    }
}
