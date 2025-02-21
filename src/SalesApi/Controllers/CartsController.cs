using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalesApi.Application.Dtos;
using SalesApi.Domain.Entities;
using SalesApi.Domain.Interfaces;

namespace SalesApi.Controllers
{
    [ApiController]
    [Route("api/carts")]
    [Produces("application/json")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public CartsController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all carts
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Cart>), 200)]
        public async Task<IActionResult> GetCarts()
        {
            var carts = await _cartService.GetCartsAsync();
            return Ok(new { data = carts, status = "success", message = "Operation completed successfully." });
        }

        /// <summary>
        /// Create a new cart
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Cart), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCart([FromBody] CartDto cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);
            var createdCart = await _cartService.CreateCartAsync(cart);

            return CreatedAtAction(nameof(GetCartById), new { id = createdCart.Id }, new { data = createdCart, status = "success", message = "Cart created successfully." });
        }

        /// <summary>
        /// Get a cart by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cart), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCartById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { status = "error", message = "Invalid ID." });

            var cart = await _cartService.GetCartByIdAsync(id);
            if (cart == null)
                return NotFound(new { status = "error", message = "Cart not found." });

            return Ok(new { data = cart, status = "success", message = "Operation completed successfully." });
        }

        /// <summary>
        /// Delete a cart
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { status = "error", message = "Invalid ID." });

            var cart = await _cartService.GetCartByIdAsync(id);
            if (cart == null)
                return NotFound(new { status = "error", message = "Cart not found." });

            await _cartService.DeleteCartAsync(id);
            return Ok(new { status = "success", message = "Cart successfully removed." });
        }
    }
}
