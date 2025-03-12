using System;
using DAY3.Models;
using Microsoft.EntityFrameworkCore;

namespace DAY3.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            // Do NOT call seed methods here!
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => op.Id);

            modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    Name = "Shubh Kachhadiya",
                    Email = "shubh@example.com",
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new Customer
                {
                    Id = 2,
                    Name = "Jay Patel",
                    Email = "jay@example.com",
                    CreatedDate = new DateTime(2024, 1, 1)
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 75000, Stock = 10 },
                new Product { Id = 2, Name = "Smartphone", Price = 25000, Stock = 25 },
                new Product { Id = 3, Name = "Headphones", Price = 3000, Stock = 50 }
            );
        }

        public void SeedDynamicData()
        {
            if (Orders.Any() || OrderProducts.Any())
            {
                Console.WriteLine("Orders or OrderProducts already exist. Skipping dynamic seed...");
                return;
            }

            var customers = Customers.ToList();
            var products = Products.ToList();

            if (!customers.Any() || !products.Any())
            {
                Console.WriteLine("No customers or products found. Seed static data first.");
                return;
            }

            var orders = new List<Order>();
            var orderProducts = new List<OrderProduct>();

            Console.WriteLine("Seeding dynamic orders...");

            foreach (var customer in customers)
            {
                for (int i = 0; i < 2; i++)
                {
                    var order = new Order
                    {
                        CustomerId = customer.Id,
                        OrderDate = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    orders.Add(order);
                }
            }

            Orders.AddRange(orders);
            SaveChanges();

            Console.WriteLine($"Added {orders.Count} orders.");

            foreach (var order in orders)
            {
                var selectedProducts = products.Take(2).ToList();
                foreach (var product in selectedProducts)
                {
                    var orderProduct = new OrderProduct
                    {
                        OrderId = order.Id,
                        ProductId = product.Id,
                        Quantity = 1
                    };
                    orderProducts.Add(orderProduct);
                }
            }

            OrderProducts.AddRange(orderProducts);
            SaveChanges();

            Console.WriteLine($"Added {orderProducts.Count} orderProducts.");
        }

    }
}
