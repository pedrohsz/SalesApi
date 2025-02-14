using WebApplication1.Application.Dtos;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Interfaces;

namespace WebApplication1.Domain.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IEnumerable<Cart>> GetCartsAsync()
        {
            return await _cartRepository.GetAllAsync();
        }

        public async Task<Cart> GetCartByIdAsync(Guid id)
        {
            return await _cartRepository.GetByIdAsync(id);
        }

        public async Task<Cart> CreateCartAsync(CartDto cartDto)
        {
            // Criamos os itens do carrinho já vinculando o CartId
            var cartItems = cartDto.Products
                .Select(p => new CartItem(p.CartId, p.ProductId, p.Quantity))
                .ToList();

            // Criamos o carrinho passando os itens diretamente
            var cart = new Cart(cartDto.UserId, cartItems);

            //cart.AddProducts(cartItems);

            // Salvamos no banco
            await _cartRepository.AddAsync(cart);
            return cart;
        }

        public async Task DeleteCartAsync(Guid id)
        {
            await _cartRepository.DeleteAsync(id);
        }
    }

}
