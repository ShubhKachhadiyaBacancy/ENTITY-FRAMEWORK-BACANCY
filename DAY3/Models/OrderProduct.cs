using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DAY3.Models
{
    public class OrderProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("OrderId")]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Navigation properties
        [JsonIgnore]
        public virtual Order Order { get; set; }
        //[JsonIgnore]
        public virtual Product Product { get; set; }
    }
}
