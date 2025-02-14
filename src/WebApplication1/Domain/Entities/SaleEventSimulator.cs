using WebApplication1.Domain.Interfaces;

namespace WebApplication1.Infrastructure.Services
{
    public class SaleEventSimulator : ISaleEventSimulator
    {
        private readonly ILogger<SaleEventSimulator> _logger;

        public SaleEventSimulator(ILogger<SaleEventSimulator> logger)
        {
            _logger = logger;
        }

        public void PublishSaleCreated(Guid saleId)
        {
            _logger.LogInformation("Simulating event: SaleCreated - SaleId: {SaleId}", saleId);
        }

        public void PublishSaleModified(Guid saleId)
        {
            _logger.LogInformation("Simulating event: SaleModified - SaleId: {SaleId}", saleId);
        }

        public void PublishSaleCancelled(Guid saleId)
        {
            _logger.LogInformation("Simulating event: SaleCancelled - SaleId: {SaleId}", saleId);
        }

        public void PublishItemCancelled(Guid saleId, Guid productId)
        {
            _logger.LogInformation("Simulating event: ItemCancelled - SaleId: {SaleId}, ProductId: {ProductId}", saleId, productId);
        }
    }
}
