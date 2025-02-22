using AutoMapper;
using SalesApi.Application.Dtos;
using SalesApi.Domain.Entities;
using SalesApi.Domain.Interfaces;

namespace SalesApi.Domain.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<SaleService> _logger;
        private readonly IMapper _mapper;
        private readonly ISaleEventSimulator _eventSimulator;

        public SaleService(
            ISaleRepository saleRepository,
            IProductRepository productRepository,
            ILogger<SaleService> logger,
            IMapper mapper,
            ISaleEventSimulator eventSimulator)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _logger = logger;
            _mapper = mapper;
            _eventSimulator = eventSimulator;
        }

        public async Task<Sale> CreateSaleAsync(SaleDto saleDto)
        {
            _logger.LogInformation("Creating a new sale for customer {CustomerId} at branch {BranchId}", saleDto.CustomerId, saleDto.BranchId);

            var saleItems = new List<SaleItem>();

            foreach (var item in saleDto.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new ArgumentException($"Product with ID {item.ProductId} does not exist.");

                var saleItem = new SaleItem(Guid.NewGuid(), item.ProductId, item.Quantity, product.Price);
                saleItems.Add(saleItem);
            }

            var sale = new Entities.Sale(saleDto.SaleNumber, saleDto.CustomerId, saleDto.BranchId, saleItems);
            await _saleRepository.AddAsync(sale);

            // Simula o envio do evento SaleCreated
            _eventSimulator.PublishSaleCreated(sale.Id);

            return sale;
        }

        public async Task<Sale> CancelSaleAsync(Guid saleId)
        {
            _logger.LogInformation("Cancelling sale {SaleId}", saleId);

            var sale = await _saleRepository.GetByIdAsync(saleId);
            if (sale == null)
            {
                _logger.LogWarning("Attempt to cancel a non-existent sale: {SaleId}", saleId);
                throw new KeyNotFoundException("Venda não encontrada.");
            }

            sale.CancelSale();
            await _saleRepository.UpdateAsync(sale);

            // Simula o envio do evento SaleCancelled
            _eventSimulator.PublishSaleCancelled(sale.Id);

            return sale;
        }

        public async Task<Sale> CancelItemAsync(Guid saleId, Guid productId)
        {
            _logger.LogInformation("Cancelling item {ProductId} from sale {SaleId}", productId, saleId);

            var sale = await _saleRepository.GetByIdAsync(saleId);
            if (sale == null)
            {
                _logger.LogWarning("Attempt to cancel an item from a non-existent sale: {SaleId}", saleId);
                throw new KeyNotFoundException("Venda não encontrada.");
            }

            sale.CancelItem(productId);
            await _saleRepository.UpdateAsync(sale);

            // Simula o envio do evento ItemCancelled
            _eventSimulator.PublishItemCancelled(sale.Id, productId);

            return sale;
        }

        public async Task<IEnumerable<Sale>> GetSalesAsync()
        {
            _logger.LogInformation("Fetching all sales.");

            var sales = await _saleRepository.GetAllAsync();
            return sales;
        }

        public async Task<Sale> GetSaleByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching sale {SaleId}", id);

            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null)
            {
                _logger.LogWarning("Sale {SaleId} not found.", id);
                throw new KeyNotFoundException("Venda não encontrada.");
            }

            return sale;
        }
    }
}
