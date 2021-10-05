using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Milkify.Core.Entities
{
    public class Inventory
    {
        [Key]
        public long Id { get; set; }
        
        public string Shelf { get; set; }

        public string SupplyCode { get; set; }

        public int SupplyQuantity { get; set; }

        public int ReservedSupplyQuantity { get; set; }

        public DateTime ExpirationDate { get; set; }

        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        public long FactoryId { get; set; }

        public virtual Factory Factory { get; set; }
    }
}
