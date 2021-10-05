using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Milkify.Core.Entities
{
    public class Product
    {
        [Key]
        public long Id { get; set; }

        public string ProductName { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public int Weight { get; set; }

        public long CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
    }
}
