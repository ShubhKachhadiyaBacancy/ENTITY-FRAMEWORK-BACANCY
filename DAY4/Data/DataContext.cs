using DAY4.Models;
using Microsoft.EntityFrameworkCore;

namespace DAY4.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }

        //Adding Tables To Database
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<EmployeeProject> EmployeeProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.DepartmentId);

                entity.Property(d => d.DepartmentName)
                    .IsRequired() 
                    .HasMaxLength(100); 

                entity.HasMany(d => d.Employees) 
                    .WithOne(e => e.Department)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade); 
            });

            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.Name)
                    .IsRequired() 
                    .HasMaxLength(100); 

                entity.Property(e => e.Email)
                    .IsRequired() 
                    .HasMaxLength(150); 

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                // Relationships
                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentId);
            });

            // Configure Project entity
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.ProjectId);

                entity.Property(p => p.ProjectName)
                    .IsRequired() 
                    .HasMaxLength(200); 

                entity.Property(p => p.StartDate)
                    .IsRequired(); 
            });

            // Configure EmployeeProject entity (Join Table)
            modelBuilder.Entity<EmployeeProject>(entity =>
            {
                entity.HasKey(ep => new { ep.EmployeeId, ep.ProjectId });

                entity.Property(ep => ep.Role)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(ep => ep.Employee)
                    .WithMany(e => e.EmployeeProjects)
                    .HasForeignKey(ep => ep.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade); 

                entity.HasOne(ep => ep.Project) 
                    .WithMany(p => p.EmployeeProjects)
                    .HasForeignKey(ep => ep.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade); 
            });

            //Seeding Data
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, DepartmentName = "HR" },
                new Department { DepartmentId = 2, DepartmentName = "IT" }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee { EmployeeId = 1, Name = "Alice", Email = "alice@example.com", DepartmentId = 1 },
                new Employee { EmployeeId = 2, Name = "Bob", Email = "bob@example.com", DepartmentId = 2 }
            );

            modelBuilder.Entity<Project>().HasData(
                new Project { ProjectId = 1, ProjectName = "Website Redesign", StartDate = new DateOnly(2025, 3, 12) },
                new Project { ProjectId = 2, ProjectName = "Data Migration", StartDate = new DateOnly(2024, 3, 12) }
            );

            modelBuilder.Entity<EmployeeProject>().HasData(
                new EmployeeProject { EmployeeId = 1, ProjectId = 1, Role = "Manager" },
                new EmployeeProject { EmployeeId = 2, ProjectId = 2, Role = "Developer" }
            );

        }
    }
}
