namespace DAY4.Models
{
    public class Project
    {
        public int ProjectId { get; set; } // Primary Key
        public string ProjectName { get; set; }
        public DateOnly StartDate { get; set; }

        // Navigation Property for Many-to-Many relationship
        public ICollection<EmployeeProject> EmployeeProjects { get; set; }
    }
}
