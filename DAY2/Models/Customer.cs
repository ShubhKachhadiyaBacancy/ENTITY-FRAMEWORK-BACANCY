using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DAY2.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name{ get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //Navigation Property
        public List<Order> orders { get; set; } = new List<Order>();
    }
}
