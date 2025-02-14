using AutoMapper;
using Microsoft.Extensions.Logging;
using WebApplication1.Application.Dtos;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Interfaces;
using WebApplication1.Infrastructure.Repositories;

namespace WebApplication1.Domain.Services
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

            if (saleDto.Items == null || !saleDto.Items.Any())
            {
                _logger.LogWarning("Attempt to create a sale with no items.");
                throw new ArgumentException("A venda deve conter pelo menos um item.");
            }

            var sale = _mapper.Map<Sale>(saleDto);
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

        public async Task<IEnumerable<SaleDto>> GetSalesAsync()
        {
            _logger.LogInformation("Fetching all sales.");

            var sales = await _saleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SaleDto>>(sales);
        }

        public async Task<SaleDto> GetSaleByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching sale {SaleId}", id);

            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null)
            {
                _logger.LogWarning("Sale {SaleId} not found.", id);
                throw new KeyNotFoundException("Venda não encontrada.");
            }

            return _mapper.Map<SaleDto>(sale);
        }
    }
}
