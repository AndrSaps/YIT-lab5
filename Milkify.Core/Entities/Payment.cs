using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Milkify.Core.Entities
{
    public class Payment
    {
        [Key]
        public long Id { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal AmountPaid { get; set; }

        public DateTime PaymentDate { get; set; }

        public long OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
