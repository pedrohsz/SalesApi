using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using WebApplication1.Application.Dtos;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Interfaces;
using WebApplication1.Domain.Services;
using WebApplication1.Infrastructure.Repositories;

namespace UnitTests.Services
{
    public class SaleServiceTests
    {
        private readonly Mock<ISaleRepository> _saleRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ISaleEventSimulator> _eventSimulatorMock;
        private readonly Mock<ILogger<SaleService>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SaleService _saleService;

        public SaleServiceTests()
        {
            _saleRepositoryMock = new Mock<ISaleRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _eventSimulatorMock = new Mock<ISaleEventSimulator>();
            _loggerMock = new Mock<ILogger<SaleService>>();
            _mapperMock = new Mock<IMapper>();

            _saleService = new SaleService(
                _saleRepositoryMock.Object,
                _productRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object,
                _eventSimulatorMock.Object
            );
        }

        [Fact]
        public async Task CreateSaleAsync_ShouldCreateSale_WhenValidDataProvided()
        {
            // Arrange
            var saleDto = new SaleDto
            {
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                Items = new List<SaleItemDto>
                {
                    new SaleItemDto { ProductId = Guid.NewGuid(), Quantity = 5, UnitPrice = 10.0m, ProductName = "Produto Teste" }
                }
            };

            var saleItems = saleDto.Items.Select(i =>
                new SaleItem(Guid.NewGuid(), i.ProductId, i.ProductName, i.Quantity, i.UnitPrice)).ToList();

            var sale = new Sale("12345", saleDto.CustomerId, saleDto.BranchId, saleItems);

            _mapperMock.Setup(m => m.Map<Sale>(saleDto)).Returns(sale);
            _saleRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Sale>())).Returns(Task.CompletedTask);

            // Act
            var result = await _saleService.CreateSaleAsync(saleDto);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Items);
            _saleRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Sale>()), Times.Once);
            _eventSimulatorMock.Verify(e => e.PublishSaleCreated(result.Id), Times.Once);
        }


        [Fact]
        public async Task CreateSaleAsync_ShouldThrowException_WhenNoItemsProvided()
        {
            // Arrange
            var saleDto = new SaleDto { CustomerId = Guid.NewGuid(), BranchId = Guid.NewGuid(), Items = new List<SaleItemDto>() };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _saleService.CreateSaleAsync(saleDto));
        }

        [Fact]
        public async Task CancelSaleAsync_ShouldCancelSale_WhenSaleExists()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var saleItem = new SaleItem(saleId, Guid.NewGuid(), "Produto Teste", 2, 50.00m);
            var sale = new Sale("12345", Guid.NewGuid(), Guid.NewGuid(), new List<SaleItem> { saleItem });

            typeof(Sale).GetProperty(nameof(Sale.Id))?.SetValue(sale, saleId);

            _saleRepositoryMock.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);
            _saleRepositoryMock.Setup(r => r.UpdateAsync(sale)).Returns(Task.CompletedTask);

            // Act
            var result = await _saleService.CancelSaleAsync(saleId);

            // Assert
            Assert.True(result.Cancelled);
            _saleRepositoryMock.Verify(r => r.UpdateAsync(sale), Times.Once);
            _eventSimulatorMock.Verify(e => e.PublishSaleCancelled(saleId), Times.Once);
        }


        [Fact]
        public async Task CancelSaleAsync_ShouldThrowException_WhenSaleDoesNotExist()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            _saleRepositoryMock.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _saleService.CancelSaleAsync(saleId));
        }

        [Fact]
        public async Task CancelItemAsync_ShouldCancelItem_WhenSaleAndItemExist()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var saleItem = new SaleItem(saleId, productId, "Product Name", 5, 10.0m);

            var sale = new Sale("12345", Guid.NewGuid(), Guid.NewGuid(), new List<SaleItem> { saleItem });

            // 🔹 Forçando o ID da venda e do produto
            typeof(Sale).GetProperty(nameof(Sale.Id))?.SetValue(sale, saleId);
            typeof(SaleItem).GetProperty(nameof(SaleItem.ProductId))?.SetValue(saleItem, productId);

            _saleRepositoryMock.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync(sale);
            _saleRepositoryMock.Setup(r => r.UpdateAsync(sale)).Returns(Task.CompletedTask);

            // Act
            var result = await _saleService.CancelItemAsync(saleId, productId);

            // Assert
            Assert.NotNull(result);
            _saleRepositoryMock.Verify(r => r.UpdateAsync(sale), Times.Once);
            _eventSimulatorMock.Verify(e => e.PublishItemCancelled(saleId, productId), Times.Once);
        }


        [Fact]
        public async Task CancelItemAsync_ShouldThrowException_WhenSaleDoesNotExist()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            _saleRepositoryMock.Setup(r => r.GetByIdAsync(saleId)).ReturnsAsync((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _saleService.CancelItemAsync(saleId, productId));
        }

        [Fact]
        public async Task GetSalesAsync_ShouldReturnSalesList()
        {
            // Arrange
            var sales = new List<Sale>
            {
                new Sale("12345", Guid.NewGuid(), Guid.NewGuid(), new List<SaleItem>
                    {
                        new SaleItem(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", 5, 10.0m) // 🔹 Agora contém pelo menos um item
                    })
            };

            var salesDto = new List<SaleDto>
            {
                new SaleDto
                    {
                        Id = sales[0].Id,
                        SaleNumber = sales[0].SaleNumber,
                        CustomerId = sales[0].CustomerId,
                        BranchId = sales[0].BranchId,
                        Items = sales[0].Items.Select(i => new SaleItemDto
                        {
                            SaleId = i.SaleId,
                            ProductId = i.ProductId,
                            Quantity = i.Quantity,
                            UnitPrice = i.UnitPrice,
                            ProductName = i.ProductName
                        }).ToList()
                    }
            };

            _saleRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(sales);
            _mapperMock.Setup(m => m.Map<IEnumerable<SaleDto>>(sales)).Returns(salesDto);

            // Act
            var result = await _saleService.GetSalesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            _saleRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}
