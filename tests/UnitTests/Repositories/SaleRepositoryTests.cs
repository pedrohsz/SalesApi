using Microsoft.EntityFrameworkCore;
using SalesApi.Domain.Entities;
using SalesApi.Infrastructure.Data;
using SalesApi.Infrastructure.Repositories;

namespace SalesApi.Tests.Repositories
{
    public class SaleRepositoryTests
    {
        private readonly DbContextOptions<SalesApiDbContext> _options;

        public SaleRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<SalesApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task Should_Add_Sale_To_Database()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new SaleRepository(context);
                var sale = new Sale("SALE123", Guid.NewGuid(), Guid.NewGuid(), new List<SaleItem>
                {
                    new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 10.0m)
                });

                await repository.AddAsync(sale);

                var savedSale = await context.Sales.FirstOrDefaultAsync(s => s.SaleNumber == "SALE123");
                Assert.NotNull(savedSale);
            }
        }

        [Fact]
        public async Task Should_Get_Sale_By_Id()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new SaleRepository(context);
                var sale = new Sale("SALE124", Guid.NewGuid(), Guid.NewGuid(), new List<SaleItem>
                {
                    new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 10, 20.0m)
                });

                await repository.AddAsync(sale);
                var retrievedSale = await repository.GetByIdAsync(sale.Id);

                Assert.NotNull(retrievedSale);
                Assert.Equal(sale.Id, retrievedSale.Id);
            }
        }

        [Fact]
        public async Task Should_Get_All_Sales()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new SaleRepository(context);

                var sale1 = new Sale("SALE125", Guid.NewGuid(), Guid.NewGuid(), new List<SaleItem>
                {
                    new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 10.0m)
                });

                var sale2 = new Sale("SALE126", Guid.NewGuid(), Guid.NewGuid(), new List<SaleItem>
                {
                    new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 3, 15.0m)
                });

                await repository.AddAsync(sale1);
                await repository.AddAsync(sale2);

                var allSales = await repository.GetAllAsync();
                Assert.Equal(2, allSales.Count());
            }
        }

        [Fact]
        public async Task Should_Update_Sale()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new SaleRepository(context);
                var sale = new Sale("SALE127", Guid.NewGuid(), Guid.NewGuid(), new List<SaleItem>
                {
                    new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 8, 10.0m)
                });

                await repository.AddAsync(sale);

                sale.CancelSale(); // Simular cancelamento
                await repository.UpdateAsync(sale);

                var updatedSale = await repository.GetByIdAsync(sale.Id);
                Assert.True(updatedSale.Cancelled);
            }
        }

        [Fact]
        public async Task Should_Delete_Sale()
        {
            using (var context = new SalesApiDbContext(_options))
            {
                var repository = new SaleRepository(context);
                var saleId = Guid.NewGuid();
                var sale = new Sale("SALE128", Guid.NewGuid(), Guid.NewGuid(), new List<SaleItem>
                {
                    new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 2, 50.0m)
                });

                await repository.AddAsync(sale);
                await repository.DeleteAsync(saleId);

                var deletedSale = await repository.GetByIdAsync(saleId);
                Assert.Null(deletedSale);
            }
        }
    }
}
