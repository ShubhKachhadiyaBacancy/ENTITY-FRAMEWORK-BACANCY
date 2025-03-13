namespace DAY4.Models
{
    public class EmployeeProject
    {
        public int EmployeeId { get; set; } // Composite Key
        public Employee Employee { get; set; } // Navigation Property

        public int ProjectId { get; set; } // Composite Key
        public Project Project { get; set; } // Navigation Property

        public string Role { get; set; } // Role in the project
    }
}
