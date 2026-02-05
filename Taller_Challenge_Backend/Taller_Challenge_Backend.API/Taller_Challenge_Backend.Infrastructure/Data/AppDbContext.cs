using Microsoft.EntityFrameworkCore;
using Taller_Challenge_Backend.Domain.Entities;

namespace Taller_Challenge_Backend.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.VehiclePlate)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.TaxAmount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.DiscountAmount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(18,2)");

                entity.HasMany(e => e.Items)
                    .WithOne()
                    .HasForeignKey(i => i.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Index creation
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => new { e.Status, e.CreatedAt });
            });

            // OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Quantity)
                    .IsRequired();

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            });
        }
    }
}