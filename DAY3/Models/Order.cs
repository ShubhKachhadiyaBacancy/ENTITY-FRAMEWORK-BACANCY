using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DAY3.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Navigation property for many-to-one relationship
        //[JsonIgnore]
        public virtual Customer Customer { get; set; }

        // Navigation property for many-to-many relationship
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
