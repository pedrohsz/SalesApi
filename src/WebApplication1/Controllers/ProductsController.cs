using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application.Dtos;
using WebApplication1.Domain.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;


        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> GetProducts(
            [FromQuery] int _page = 1,
            [FromQuery] int _size = 10,
            [FromQuery] string? _order = null)
        {
            _logger.LogInformation("Fetching products - Page: {Page}, Size: {Size}, Order: {Order}", _page, _size, _order);

            var (products, totalItems, totalPages) = await _productService.GetProductsPagedAsync(_page, _size, _order);

            return Ok(new
            {
                data = products,
                totalItems,
                currentPage = _page,
                totalPages,
                status = "success",
                message = "Operação concluída com sucesso"
            });
        }

        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _productService.GetCategoriesAsync();
            return Ok(categories);
        }

        // <summary>
        /// Obtém produtos de uma categoria específica com paginação e ordenação.
        /// </summary>
        [HttpGet("category/{category}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductsByCategory(string category, [FromQuery] int _page = 1, [FromQuery] int _size = 10, [FromQuery] string? _order = null)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                _logger.LogWarning("Category name is missing in the request.");
                return BadRequest(new { status = "error", message = "Nome da categoria é obrigatório." });
            }

            var (products, totalItems, totalPages) = await _productService.GetProductsByCategoryAsync(category, _page, _size, _order);

            return Ok(new
            {
                data = products,
                totalItems,
                currentPage = _page,
                totalPages,
                status = "success",
                message = "Operação concluída com sucesso"
            });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            var product = await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, new
            {
                data = product,
                status = "success",
                message = "Operação concluída com sucesso"
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { status = "error", message = "Produto não encontrado" });

            return Ok(new { data = product, status = "success", message = "Operação concluída com sucesso" });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                _logger.LogWarning("Product update failed: Request body is null.");
                return BadRequest(new { status = "error", message = "Dados inválidos. O corpo da requisição está vazio." });
            }

            var updatedProduct = await _productService.UpdateProductAsync(id, productDto);

            if (updatedProduct == null)
            {
                _logger.LogWarning("Product update failed: Product with ID {ProductId} not found.", id);
                return NotFound(new { status = "error", message = "Produto não encontrado." });
            }

            _logger.LogInformation("Product {ProductId} updated successfully.", id);
            return Ok(new { data = updatedProduct, status = "success", message = "Produto atualizado com sucesso" });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var deleted = await _productService.DeleteProductAsync(id);

            if (!deleted)
            {
                _logger.LogWarning("Product deletion failed: Product with ID {ProductId} not found.", id);
                return NotFound(new { status = "error", message = "Produto não encontrado." });
            }

            _logger.LogInformation("Product {ProductId} deleted successfully.", id);
            return Ok(new { status = "success", message = "Produto removido com sucesso" });
        }
    }

}
