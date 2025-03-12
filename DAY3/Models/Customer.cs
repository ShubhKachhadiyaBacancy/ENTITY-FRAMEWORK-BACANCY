using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DAY3.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [DefaultValue(false)]
        public bool IsVIP { get; set; }

        // Navigation property for one-to-many relationship
        public virtual ICollection<Order> Orders { get; set; }
    }
}
