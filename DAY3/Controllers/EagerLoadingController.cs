using DAY3.Data;
using DAY3.Dtos;
using DAY3.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAY3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EagerLoadingController : Controller
    {
        private readonly DataContext _dataContext;
        public EagerLoadingController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("GetCustomersWithOrdersAndProducts")]
        public async Task<IActionResult> GetCustomersWithOrdersAndProducts()
        {
            var customersWithOrdersAndProducts = await _dataContext.Customers
            .Include(c => c.Orders)
                .ThenInclude(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
            .Select(c => new CustomerDTO
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                CreatedDate = c.CreatedDate,
                Orders = c.Orders.Select(o => new OrderDTO
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    IsDeleted = o.IsDeleted,
                    OrderProducts = o.OrderProducts.Select(op => new OrderProductDTO
                    {
                        Id = op.Id,
                        OrderId = op.OrderId,
                        ProductId = op.ProductId,
                        Quantity = op.Quantity,
                        ProductName = op.Product.Name
                    }).ToList()
                }).ToList()
            }).ToListAsync();

            return Ok(customersWithOrdersAndProducts);
        }

        [HttpGet("GetRecentCustomersWithFilteredProducts")]
        public async Task<IActionResult> GetRecentCustomersWithFilteredProducts()
        {
            var recentCustomersWithFilteredProducts = await _dataContext.Customers
            .Include(c => c.Orders)
                .ThenInclude(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
            .Select(c => new CustomerDTO
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                CreatedDate = c.CreatedDate,
                Orders = c.Orders
                    .Where(o => o.OrderDate >= DateTime.UtcNow.AddDays(-30))
                    .Select(o => new OrderDTO
                    {
                        Id = o.Id,
                        OrderDate = o.OrderDate,
                        IsDeleted = o.IsDeleted,
                        OrderProducts = o.OrderProducts
                            .Where(op => op.Product.Stock > 20)
                            .Select(op => new OrderProductDTO
                            {
                                Id = op.Id,
                                OrderId = op.OrderId,
                                ProductId = op.ProductId,
                                Quantity = op.Quantity,
                                ProductName = op.Product.Name
                            }).ToList()
                    }).ToList()
            }).ToListAsync();

            return Ok(recentCustomersWithFilteredProducts);
        }

        [HttpGet("GetProductsWithOrderCount")]
        public async Task<IActionResult> GetProductsWithOrderCount()
        {
            var productsWithOrderCount = await _dataContext.Products
            .Include(p => p.OrderProducts)
            .ThenInclude(op => op.Order)
            .Select(p => new
            {
                Product = new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock
                },
                TotalOrders = p.OrderProducts.Count
            })
            .ToListAsync();

            return Ok(productsWithOrderCount);
        }
        
        [HttpGet("GetRecentOrdersWithCustomers")]
        public async Task<IActionResult> GetRecentOrdersWithCustomers()
        {
            var recentOrdersWithCustomers = await _dataContext.Orders
            .Include(o => o.Customer)
            .Where(o => o.OrderDate >= DateTime.UtcNow.AddMonths(-1))
            .Select(o => new OrderDTO
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                IsDeleted = o.IsDeleted,
                CustomerId = o.CustomerId,
                OrderProducts = new List<OrderProductDTO>(),
            }).ToListAsync();

            return Ok(recentOrdersWithCustomers);
        }
    }
}
