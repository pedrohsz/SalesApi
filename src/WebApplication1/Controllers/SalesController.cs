using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application.Dtos;
using WebApplication1.Domain.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/sales")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly ILogger<SalesController> _logger;
        private readonly IMapper _mapper;

        public SalesController(ISaleService saleService, ILogger<SalesController> logger, IMapper mapper)
        {
            _saleService = saleService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Cria uma nova venda.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] SaleDto saleDto)
        {
            _logger.LogInformation("Recebida solicitação para criar uma nova venda.");

            if (saleDto == null || saleDto.Items == null || !saleDto.Items.Any())
            {
                _logger.LogWarning("Tentativa de criação de venda sem itens.");
                return BadRequest(new { status = "error", message = "A venda deve conter pelo menos um item." });
            }

            var sale = await _saleService.CreateSaleAsync(saleDto);
            var response = _mapper.Map<SaleDto>(sale);

            return CreatedAtAction(nameof(GetSaleById), new { id = response.Id },
                new { data = response, status = "success", message = "Venda criada com sucesso" });
        }

        /// <summary>
        /// Retorna todas as vendas cadastradas.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSales()
        {
            _logger.LogInformation("Recebida solicitação para listar todas as vendas.");

            var sales = await _saleService.GetSalesAsync();
            return Ok(new { data = sales, status = "success", message = "Operação concluída com sucesso" });
        }

        /// <summary>
        /// Retorna uma venda específica pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSaleById(Guid id)
        {
            _logger.LogInformation("Buscando venda com ID {SaleId}", id);

            var sale = await _saleService.GetSaleByIdAsync(id);
            if (sale == null)
            {
                _logger.LogWarning("Venda com ID {SaleId} não encontrada.", id);
                return NotFound(new { status = "error", message = "Venda não encontrada" });
            }

            return Ok(new { data = sale, status = "success", message = "Operação concluída com sucesso" });
        }

        /// <summary>
        /// Cancela uma venda pelo ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelSale(Guid id)
        {
            _logger.LogInformation("Solicitação para cancelar venda {SaleId}", id);

            var sale = await _saleService.CancelSaleAsync(id);
            return Ok(new { data = sale, status = "success", message = "Venda cancelada com sucesso" });
        }

        /// <summary>
        /// Cancela um item específico dentro de uma venda.
        /// </summary>
        [HttpDelete("{saleId}/items/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelItem(Guid saleId, Guid productId)
        {
            _logger.LogInformation("Solicitação para cancelar item {ProductId} da venda {SaleId}", productId, saleId);

            var sale = await _saleService.CancelItemAsync(saleId, productId);
            return Ok(new { data = sale, status = "success", message = "Item cancelado com sucesso" });
        }
    }
}