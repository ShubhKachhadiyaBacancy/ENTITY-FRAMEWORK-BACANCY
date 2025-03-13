using System.ComponentModel.DataAnnotations;

namespace DAY4.DTOs
{
    public class CreateProjectDTO
    {
        [Required]
        public string ProjectName { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }
    }
}
