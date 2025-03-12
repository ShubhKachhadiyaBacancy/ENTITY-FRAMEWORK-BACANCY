using DAY3.Data;
using Microsoft.AspNetCore.Mvc;

namespace DAY3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LazyLoadingController : Controller
    {
        private readonly DataContext _context;

        public LazyLoadingController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("CustomersWithOrders")]
        public IActionResult GetCustomersWithOrders()
        {
            var customers = _context.Customers.ToList();
            var result = new List<object>();

            foreach (var customer in customers)
            {
                var orders = customer.Orders.Select(o => new
                {
                    o.Id,
                    o.OrderDate
                }).ToList();

                result.Add(new
                {
                    customer.Id,
                    customer.Name,
                    customer.Email,
                    Orders = orders
                });
            }

            return Ok(result);
        }

        [HttpGet("CustomersWithHighValueOrders")]
        public IActionResult GetCustomersWithHighValueOrders()
        {
            var customers = _context.Customers.ToList(); 
            var result = new List<object>();

            foreach (var customer in customers)
            {
                var highValueOrders = customer.Orders
                    .Where(order => order.OrderProducts
                        .Sum(op => op.Quantity * op.Product.Price) > 500*85)
                    .Select(o => new
                    {
                        o.Id,
                        o.OrderDate,
                        TotalAmount = o.OrderProducts.Sum(op => op.Quantity * op.Product.Price)
                    })
                    .ToList();

                if (highValueOrders.Any())
                {
                    result.Add(new
                    {
                        customer.Id,
                        customer.Name,
                        HighValueOrders = highValueOrders
                    });
                }
                else
                {
                    return BadRequest("NO ORDERS FOUND WITH VALUE > $500");
                }
            }

            return Ok(result);
        }
    }
}
