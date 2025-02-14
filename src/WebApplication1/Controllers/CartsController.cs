using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application.Dtos;
using WebApplication1.Domain.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/carts")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarts()
        {
            var carts = await _cartService.GetCartsAsync();
            return Ok(new { data = carts, status = "success", message = "Operação concluída com sucesso" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] CartDto cartDto)
        {
            var cart = await _cartService.CreateCartAsync(cartDto);
            return CreatedAtAction(nameof(GetCartById), new { id = cart.Id }, cart);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartById(Guid id)
        {
            var cart = await _cartService.GetCartByIdAsync(id);
            if (cart == null)
                return NotFound(new { status = "error", message = "Carrinho não encontrado" });

            return Ok(new { data = cart, status = "success", message = "Operação concluída com sucesso" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            await _cartService.DeleteCartAsync(id);
            return Ok(new { status = "success", message = "Carrinho removido com sucesso" });
        }
    }

}
