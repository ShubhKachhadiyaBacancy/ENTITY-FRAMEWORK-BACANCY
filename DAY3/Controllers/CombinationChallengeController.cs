using DAY3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAY3.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CombinationChallengeController : Controller
    {
        private readonly DataContext _context;

        public CombinationChallengeController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("EagerLazy")]
        public IActionResult GetOrdersWithEagerCustomerAndLazyOrderProducts()
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                .Where(o => o.OrderDate > DateTime.Now.AddMonths(-1))
                .ToList();

            var result = new List<object>();

            foreach (var order in orders)
            {
                var orderProducts = order.OrderProducts.Select(op => new
                {
                    op.ProductId,
                    op.Quantity
                });

                result.Add(new
                {
                    OrderId = order.Id,
                    CustomerName = order.Customer.Name,
                    OrderProducts = orderProducts 
                });
            }

            return Ok(result);
        }

        [HttpGet("EagerExplicit")]
        public IActionResult GetOrdersWithExplicitOrderProducts()
        {
            var orders = _context.Orders
            .Include(o => o.Customer) 
            .Where(o => o.OrderProducts.Any(op => op.Quantity >= 5)) 
            .ToList();

            foreach (var order in orders)
            {
                _context.Entry(order)
                    .Collection(o => o.OrderProducts)
                    .Query()
                    .Where(op => op.Quantity >= 5)
                    .Load();
            }

            var result = orders.Select(order => new
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                CustomerName = order.Customer.Name,
                OrderProducts = order.OrderProducts.Select(op => new
                {
                    ProductId = op.ProductId,
                    Quantity = op.Quantity
                }).ToList()
            });

            return Ok(result);
        }

        [HttpGet("GetVipCustomers")]
        public IActionResult GetVipCustomers()
        {
            var vipCustomers = _context.Customers
                .Where(c => c.IsVIP)
                .Include(c => c.Orders)
                .ToList();

            var result = vipCustomers.Select(customer => new
            {
                CustomerId = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                CreatedDate = customer.CreatedDate,
                Orders = customer.Orders.Select(order => new
                {
                    OrderId = order.Id,
                    OrderDate = order.OrderDate,
                    OrderTotal = order.OrderProducts.Sum(op => op.Quantity * op.Product.Price) 
                })
            });

            return Ok(result);
        }

    }
}
