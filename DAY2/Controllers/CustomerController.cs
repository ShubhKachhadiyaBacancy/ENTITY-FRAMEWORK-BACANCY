using DAY2.Models;
using Microsoft.AspNetCore.Mvc;
using DAY2.DataContext;

namespace DAY2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController: ControllerBase
    {
        private readonly EFCoreDataContext _context;

        public CustomerController(EFCoreDataContext context)
        {
            _context = context;
        }

        [HttpGet("GetCustomers")]
        public IActionResult GetCustomers()
        {
            if(_context.customers.ToList().Count == 0)
            {
                return BadRequest("ERROR");
            }
            var customers = _context.customers.ToList();
            if(customers == null || customers.Count == 0)
            {
                return BadRequest("NO CUSTOMERS FOUND");
            }
            return Ok(customers);
        }

        [HttpPost("AddCustomer")]
        public IActionResult CreateCustomer([FromBody] Customer customer)
        {
            _context.customers.Add(customer);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCustomers), new { id = customer.CustomerId }, customer);
        }

        [HttpDelete("HardDeleteCustomer")]
        public IActionResult HardDeleteCustomer([FromBody] Customer customer)
        {
            var c = _context.customers.Find(customer.CustomerId);
            if(c == null)
            {
                return BadRequest("CUSTOMER NOT FOUND");
            }

            _context.customers.Remove(c);
            _context.SaveChanges();
            return Ok("CUSTOMER HARD DELETED");
        }

        [HttpDelete("SoftDeleteCustomer")]
        public IActionResult SoftDeleteCustomer([FromBody] Customer customer)
        {
            var c = _context.customers.Find(customer.CustomerId);
            if (c == null)
            {
                return BadRequest("CUSTOMER NOT FOUND");
            }

            c.IsDeleted = true;
            _context.SaveChanges();
            return Ok("CUSTOMER SOFT DELETED");
        }
    }
}

