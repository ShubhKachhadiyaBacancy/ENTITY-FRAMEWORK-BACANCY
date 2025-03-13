using System.ComponentModel.DataAnnotations;

namespace DAY4.DTOs
{
    public class CreateEmployeeDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}
