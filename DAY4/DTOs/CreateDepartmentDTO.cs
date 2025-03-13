using System.ComponentModel.DataAnnotations;

namespace DAY4.DTOs
{
    public class CreateDepartmentDTO
    {
        [Required]
        public string DepartmentName { get; set; }
    }
}
