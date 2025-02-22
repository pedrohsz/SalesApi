using Microsoft.EntityFrameworkCore;
using SalesApi.Domain.Entities;

namespace SalesApi.Infrastructure.Data
{
    public class SalesApiDbContext : DbContext
    {
        public SalesApiDbContext(DbContextOptions<SalesApiDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Sale>()
                .HasMany(s => s.Items)
                .WithOne()
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sale>()
                .Property(s => s.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<SaleItem>()
                .HasOne<Sale>()
                .WithMany(s => s.Items)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SaleItem>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(si => si.ProductId);

            modelBuilder.Entity<SaleItem>()
                .Property(si => si.Quantity)
                .IsRequired();

            modelBuilder.Entity<SaleItem>()
                .Property(si => si.UnitPrice)
                .IsRequired();

            modelBuilder.Entity<SaleItem>()
                .Property(si => si.Discount)
                .HasDefaultValue(0);

            modelBuilder.Entity<SaleItem>()
                .Property(si => si.IsCancelled)
                .HasDefaultValue(false);

            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<CartItem>()
                .HasKey(ci => ci.Id);

            modelBuilder.Entity<CartItem>()
                .HasOne<Cart>()
                .WithMany(c => c.Products)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(ci => ci.ProductId);

            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.Quantity)
                .IsRequired();
        }
    }
}
