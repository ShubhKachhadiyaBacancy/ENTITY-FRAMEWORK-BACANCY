using DAY2.Models;
using Microsoft.EntityFrameworkCore;

namespace DAY2.DataContext
{
    public class EFCoreDataContext:DbContext
    {
        public EFCoreDataContext(DbContextOptions<EFCoreDataContext> options):base(options)
        {
        }

        public DbSet<Order> orders { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<OrderProduct> orderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships and composite keys
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.order)
                .WithMany(o => o.orderProducts)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.product)
                .WithMany(p => p.orderProducts)
                .HasForeignKey(op => op.ProductId);

            modelBuilder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted );
        }
    }
}
