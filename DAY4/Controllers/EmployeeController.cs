using DAY4.Data;
using DAY4.DTOs;
using DAY4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAY4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly DataContext _context;

        public EmployeeController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("RegisterEmployee")]
        public IActionResult CreateEmployee([FromBody] CreateEmployeeDTO employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Employee data is required.");
            }

            if (_context.Employees.Any(e => e.Email == employeeDto.Email))
            {
                return Conflict("An employee with the given email already exists.");
            }

            if (_context.Departments.All(d => d.DepartmentId != employeeDto.DepartmentId))
            {
                return NotFound("Department Id Not Found");
            }

            var employee = new Employee
            {
                Email = employeeDto.Email,
                Name = employeeDto.Name,
                DepartmentId = employeeDto.DepartmentId
            };

            //METHOD-1 
            _context.Employees.Add(employee);

            //METHOD-2
            //_context.Add(employee);
            _context.SaveChanges();

            return Ok($"EMPLOYEE {employeeDto.Name} ADDED");
        }

        [HttpGet("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {
            var employees = _context.Employees.Select(e => new { 
                e.EmployeeId,
                EmployeeName = e.Name,
                e.DepartmentId
            })
            .AsNoTracking()
            .ToList();

            if (!employees.Any())
            {
                return NotFound("No employees found.");
            }

            return Ok(employees);
        }

        [HttpGet("GetEmployeeById/{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.EmployeeProjects)
                    .ThenInclude(ep => ep.Project)
                .Where(e => e.EmployeeId == id)
                .Select(e => new
                {
                    e.EmployeeId,
                    EmployeeName = e.Name,
                    EmployeeEmail = e.Email,
                    e.DepartmentId,
                    e.Department.DepartmentName,
                    EmployeeProjects = e.EmployeeProjects.Select(p => new
                    {
                        p.Project.ProjectName,
                        p.Role,
                        p.Project.StartDate
                    })
                });

            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            return Ok(employee);
        }

        [HttpPut("UpdateEmployeeById/{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] CreateEmployeeDTO updatedEmployee)
        {
            if (updatedEmployee == null)
            {
                return BadRequest("Invalid employee data.");
            }

            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            if (_context.Departments.All(d => d.DepartmentId != updatedEmployee.DepartmentId))
            {
                return NotFound($"Department with ID {updatedEmployee.DepartmentId} not found.");
            }

            employee.Name = updatedEmployee.Name;
            employee.Email = updatedEmployee.Email;
            employee.DepartmentId = updatedEmployee.DepartmentId;

            _context.SaveChanges();
            return Ok($"EMPLOYEE {id} UPDATED ");
        }

        [HttpDelete("DeleteEmployeeById/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found.");
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return Ok($"EMPLOYEE {id} DELETED");
        }
    }
}
