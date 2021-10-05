using System;
using Milkify.Core.Attributes;

namespace Milkify.Core.Dtos
{
    public class PaymentDto
    {
        public long Id { get; set; }

        [TableColumn("Amount")]
        public decimal AmountPaid { get; set; }

        [TableColumn("Payment Date")]
        public DateTime PaymentDate { get; set; }

        public long OrderId { get; set; }

        [TableColumn("Order Number", "OrderNumber")]
        public OrderDto Order { get; set; }
    }
}