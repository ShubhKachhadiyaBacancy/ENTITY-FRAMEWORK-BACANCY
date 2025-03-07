using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DAY2.Models
{
    public class OrderProduct
    {

        public int OrderId { get; set; }
        public int ProductId { get; set; }

        //Navigation Property
        public Order order { get; set; }
        public Product product { get; set; }
    }
}
