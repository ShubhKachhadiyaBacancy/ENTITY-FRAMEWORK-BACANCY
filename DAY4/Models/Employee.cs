namespace DAY4.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; } // Primary Key
        public string Name { get; set; }
        public string Email { get; set; }

        // Foreign Key for Department
        public int DepartmentId { get; set; }
        public Department Department { get; set; } // Navigation Property

        // Navigation Property for Many-to-Many relationship
        public ICollection<EmployeeProject> EmployeeProjects { get; set; }
    }
}
