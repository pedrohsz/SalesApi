using SalesApi.Domain.Entities;
using SalesApi.Domain.Interfaces;

namespace SalesApi.Domain.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Cart>> GetCartsAsync()
        {
            return await _cartRepository.GetAllAsync();
        }

        public async Task<Cart> GetCartByIdAsync(Guid id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            return cart ?? throw new KeyNotFoundException($"Cart with ID {id} not found.");
        }

        public async Task<Cart> CreateCartAsync(Cart cart)
        {
            if (cart == null || !cart.Items.Any())
                throw new ArgumentException("Cart must contain at least one item.");

            foreach (var item in cart.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new ArgumentException($"Product with ID {item.ProductId} does not exist.");
            }

            await _cartRepository.AddAsync(cart);
            return cart;
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            await _cartRepository.UpdateAsync(cart);
        }

        public async Task DeleteCartAsync(Guid id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            if (cart == null)
                throw new KeyNotFoundException($"Cart with ID {id} not found.");

            await _cartRepository.DeleteAsync(id);
        }
    }
}