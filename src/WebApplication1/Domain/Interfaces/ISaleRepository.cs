using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.Interfaces
{
    public interface ISaleRepository
    {
        /// <summary>
        /// Retorna todas as vendas cadastradas.
        /// </summary>
        Task<IEnumerable<Sale>> GetAllAsync();

        /// <summary>
        /// Retorna uma venda específica pelo ID.
        /// </summary>
        Task<Sale> GetByIdAsync(Guid id);

        /// <summary>
        /// Adiciona uma nova venda ao banco de dados.
        /// </summary>
        Task AddAsync(Sale sale);

        /// <summary>
        /// Atualiza uma venda existente no banco de dados.
        /// </summary>
        Task UpdateAsync(Sale sale);

        /// <summary>
        /// Remove uma venda do banco de dados.
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Cancela um item específico dentro de uma venda.
        /// </summary>
        Task CancelItemAsync(Guid saleId, Guid productId);
    }
}