﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DAY3.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        // Navigation property for many-to-many relationship
        //[JsonIgnore]
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
