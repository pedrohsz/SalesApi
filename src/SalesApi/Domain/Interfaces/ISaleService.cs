using SalesApi.Application.Dtos;
using SalesApi.Domain.Entities;

namespace SalesApi.Domain.Interfaces
{
    public interface ISaleService
    {
        /// <summary>
        /// Cria uma nova venda e simula o evento SaleCreated.
        /// </summary>
        /// <param name="saleDto">Dados da venda.</param>
        /// <returns>Venda criada.</returns>
        Task<Sale> CreateSaleAsync(SaleDto saleDto);

        /// <summary>
        /// Obtém todas as vendas registradas.
        /// </summary>
        /// <returns>Lista de vendas.</returns>
        Task<IEnumerable<SaleDto>> GetSalesAsync();

        /// <summary>
        /// Obtém uma venda específica pelo ID.
        /// </summary>
        /// <param name="id">ID da venda.</param>
        /// <returns>Dados da venda.</returns>
        Task<SaleDto> GetSaleByIdAsync(Guid id);

        /// <summary>
        /// Cancela uma venda e simula o evento SaleCancelled.
        /// </summary>
        /// <param name="saleId">ID da venda a ser cancelada.</param>
        /// <returns>Venda atualizada.</returns>
        Task<Sale> CancelSaleAsync(Guid saleId);

        /// <summary>
        /// Cancela um item específico dentro de uma venda e simula o evento ItemCancelled.
        /// </summary>
        /// <param name="saleId">ID da venda.</param>
        /// <param name="productId">ID do produto a ser removido.</param>
        /// <returns>Venda atualizada.</returns>
        Task<Sale> CancelItemAsync(Guid saleId, Guid productId);
    }
}
