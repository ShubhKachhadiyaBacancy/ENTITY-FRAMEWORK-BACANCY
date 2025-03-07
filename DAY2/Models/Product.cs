using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAY2.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName="Decimal(10,2)")]
        public decimal Price { get; set; }

        //Navigation Property
        public List<OrderProduct> orderProducts { get; set; } = new List<OrderProduct>();
    }
}
