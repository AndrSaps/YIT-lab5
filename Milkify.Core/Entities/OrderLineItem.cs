using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Milkify.Core.Entities
{
    public class OrderLineItem
    {
        [Key]
        public long Id { get; set; }

        public int ProductQuantity { get; set; }

        public long OrderId { get; set; }

        public virtual Order Order { get; set; }

        public long ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
