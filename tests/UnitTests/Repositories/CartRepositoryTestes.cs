using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesApi.Domain.Entities;
using SalesApi.Infrastructure.Data;
using SalesApi.Infrastructure.Repositories;
using Xunit;

namespace SalesApi.Tests.Repositories
{
    public class CartRepositoryTests
    {
        private readonly DbContextOptions<SalesApiDbContext> _options;

        public CartRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<SalesApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Cart")
                .Options;
        }

        [Fact]
        public async Task Should_Get_All_Carts()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new CartRepository(context);

                await repository.AddAsync(new Cart(Guid.NewGuid(), new List<CartItem>
                {
                    new CartItem(Guid.NewGuid(), Guid.NewGuid(), 1)
                }));

                await repository.AddAsync(new Cart(Guid.NewGuid(), new List<CartItem>
                {
                    new CartItem(Guid.NewGuid(), Guid.NewGuid(), 5)
                }));

                var allCarts = await repository.GetAllAsync();
                Assert.Equal(2, allCarts.Count());
            }
        }

        [Fact]
        public async Task Should_Add_Cart_To_Database()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new CartRepository(context);
                var cart = new Cart(Guid.NewGuid(), new List<CartItem>
                {
                    new CartItem(Guid.NewGuid(), Guid.NewGuid(), 2)
                });

                await repository.AddAsync(cart);

                var savedCart = await context.Carts.FirstOrDefaultAsync();
                Assert.NotNull(savedCart);
            }
        }

        [Fact]
        public async Task Should_Get_Cart_By_Id()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new CartRepository(context);
                var cart = new Cart(Guid.NewGuid(), new List<CartItem>
                {
                    new CartItem(Guid.NewGuid(), Guid.NewGuid(), 3)
                });

                await repository.AddAsync(cart);
                var retrievedCart = await repository.GetByIdAsync(cart.Id);

                Assert.NotNull(retrievedCart);
                Assert.Equal(cart.Id, retrievedCart.Id);
            }
        }

        [Fact]
        public async Task Should_Delete_Cart()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new CartRepository(context);
                var cartId = Guid.NewGuid();
                var cart = new Cart(cartId, new List<CartItem>
                {
                    new CartItem(Guid.NewGuid(), Guid.NewGuid(), 2)
                });

                await repository.AddAsync(cart);
                await repository.DeleteAsync(cartId);

                var deletedCart = await repository.GetByIdAsync(cartId);
                Assert.Null(deletedCart);
            }
        }
    }
}
