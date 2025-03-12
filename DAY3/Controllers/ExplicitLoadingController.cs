using DAY3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAY3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExplicitLoadingController : Controller
    {
        private readonly DataContext _context;

        public ExplicitLoadingController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("LoadCustomerOrdersLastYear")]
        public IActionResult GetCustomerOrdersCreatedLastYear(int customerId)
        {
            var customer = _context.Customers.Find(customerId);

            if (customer != null && customer.CreatedDate >= DateTime.Now.AddYears(-1))
            {
                _context.Entry(customer).Collection(c => c.Orders).Load();
            }

            var result = new
            {
                CustomerId = customer?.Id,
                Name = customer?.Name,
                CreatedDate = customer?.CreatedDate,
                Orders = customer?.Orders.Select(order => new
                {
                    OrderId = order.Id,
                    OrderDate = order.OrderDate
                }).ToList()
            };

            return Ok(result);
        }

        [HttpGet("OrdersWithoutOrderProducts")]
        public IActionResult GetOrdersWithoutOrderProducts()
        {
            var orders = _context.Orders.ToList();

            var result = orders.Select(order => new
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                IsDeleted = order.IsDeleted,
                LoadOrderProducts = $"Call '/api/ExplicitLoading/LoadOrderProducts/{order.Id}' to load OrderProducts"
            });

            return Ok(result);
        }

        [HttpGet("LoadOrderProducts/{orderId}")]
        public IActionResult LoadOrderProducts(int orderId)
        {
            var order = _context.Orders.Find(orderId);

            if (order != null)
            {
                _context.Entry(order).Collection(o => o.OrderProducts).Load();
            }

            var result = new
            {
                OrderId = order?.Id,
                OrderProducts = order?.OrderProducts.Select(op => new
                {
                    ProductId = op.ProductId,
                    Quantity = op.Quantity
                }).ToList()
            };

            return Ok(result);
        }

        [HttpGet("ProductsWithLowStock")]
        public IActionResult GetProductsWithLowStock()
        {
            var products = _context.Products.Where(p => p.Stock <= 10).ToList();

            foreach (var product in products)
            {
                _context.Entry(product).Collection(p => p.OrderProducts)
                    .Query()
                    .Include(op => op.Order)
                    .Load();
            }

            var result = products.Select(product => new
            {
                ProductId = product.Id,
                Name = product.Name,
                Stock = product.Stock,
                AssociatedOrders = product.OrderProducts.Select(op => new
                {
                    OrderId = op.OrderId,
                    Quantity = op.Quantity
                }).ToList()
            });

            return Ok(result);
        }

        [HttpGet("CustomersWithOrdersExplicitOrderProducts")]
        public IActionResult GetCustomersWithOrdersExplicitOrderProducts()
        {
            var customers = _context.Customers
                .Include(c => c.Orders)
                .ToList();

            foreach (var customer in customers)
            {
                foreach (var order in customer.Orders)
                {
                    _context.Entry(order).Collection(o => o.OrderProducts).Load();
                }
            }

            var result = customers.Select(customer => new
            {
                CustomerId = customer.Id,
                Name = customer.Name,
                Orders = customer.Orders.Select(order => new
                {
                    OrderId = order.Id,
                    OrderDate = order.OrderDate,
                    OrderProducts = order.OrderProducts.Select(op => new
                    {
                        ProductId = op.ProductId,
                        Quantity = op.Quantity
                    }).ToList()
                }).ToList()
            });

            return Ok(result);
        }
    }
}
