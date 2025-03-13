using DAY4.Data;
using DAY4.DTOs;
using DAY4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAY4.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly DataContext _context;

        public ProjectController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("RegisterProject")]
        public IActionResult CreateProject([FromBody] CreateProjectDTO projectDto)
        {
            if (projectDto == null)
            {
                return BadRequest("Project data is required.");
            }

            var project = new Project
            {
                ProjectName = projectDto.ProjectName,
                StartDate = projectDto.StartDate
            };

            _context.Projects.Add(project);
            _context.SaveChanges();

            return Ok($"PROJECT {projectDto.ProjectName} ADDED");
        }

        [HttpGet("GetAllProjects")]
        public IActionResult GetAllProjects()
        {
            var projects = _context.Projects.Select(p => new { 
                p.ProjectId,
                p.ProjectName,
                p.StartDate
            })
            .AsNoTracking()
            .ToList();

            if (!projects.Any())
            {
                return NotFound("No projects found.");
            }

            return Ok(projects);
        }

        [HttpGet("GetProjectById/{id}")]
        public IActionResult GetProjectById(int id)
        {
            var project = _context.Projects
                .Include(p => p.EmployeeProjects)
                    .ThenInclude(ep => ep.Employee)
                .Where(p => p.ProjectId == id)
                .Select(p => new
                {
                    p.ProjectId,
                    p.ProjectName,
                    p.StartDate,
                    EmployeeProjects = p.EmployeeProjects.Select(ep => new
                    {
                        ep.EmployeeId,
                        EmployeeName = ep.Employee.Name,
                        EmployeeEmail = ep.Employee.Email
                    })
                });

            if (project == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }

            return Ok(project);
        }

        [HttpGet("GetProjectsByPaging")]
        public IActionResult GetProjects([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var projects = _context.Projects
                .Include(p => p.EmployeeProjects)
                    .ThenInclude(ep => ep.Employee)
                 .Select(p => new
                 {
                     p.ProjectId,
                     p.ProjectName,
                     p.StartDate,
                     EmployeeProjects = p.EmployeeProjects.Select(ep => new
                     {
                         ep.EmployeeId,
                         EmployeeName = ep.Employee.Name,
                         EmployeeEmail = ep.Employee.Email
                     })
                 })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(projects);
        }

        [HttpPut("UpdateProjectById/{id}")]
        public IActionResult UpdateProject(int id, [FromBody] CreateProjectDTO updatedProject)
        {
            if (updatedProject == null)
            {
                return BadRequest("Invalid project data.");
            }

            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }

            project.ProjectName = updatedProject.ProjectName;
            project.StartDate = updatedProject.StartDate;

            _context.SaveChanges();
            return Ok(project);
        }

        [HttpDelete("DeleteProjectById/{id}")]
        public IActionResult DeleteProject(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound($"Project with ID {id} not found.");
            }

            _context.Projects.Remove(project);
            _context.SaveChanges();
            return Ok($"PROJECT {id} DELETED");
        }
    }
}
