using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DAY2.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        //Navigation Property
        public Customer Customer { get; set; }
        public List<OrderProduct> orderProducts { get; set; } = new List<OrderProduct>();

    }
}
