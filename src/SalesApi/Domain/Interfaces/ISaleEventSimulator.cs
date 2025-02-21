namespace SalesApi.Domain.Interfaces
{
    public interface ISaleEventSimulator
    {
        /// <summary>
        /// Simula a publicação do evento SaleCreated.
        /// </summary>
        /// <param name="saleId">ID da venda criada.</param>
        void PublishSaleCreated(Guid id);

        /// <summary>
        /// Simula a publicação do evento SaleModified.
        /// </summary>
        /// <param name="saleId">ID da venda modificada.</param>
        void PublishSaleModified(Guid saleId);

        /// <summary>
        /// Simula a publicação do evento SaleCancelled.
        /// </summary>
        /// <param name="saleId">ID da venda cancelada.</param>
        void PublishSaleCancelled(Guid saleId);

        /// <summary>
        /// Simula a publicação do evento ItemCancelled.
        /// </summary>
        /// <param name="saleId">ID da venda relacionada ao item cancelado.</param>
        /// <param name="productId">ID do produto cancelado.</param>
        void PublishItemCancelled(Guid saleId, Guid productId);
    }
}
