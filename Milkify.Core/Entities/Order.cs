using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Milkify.Core.Entities.Membership;
using Milkify.Core.Enums;

namespace Milkify.Core.Entities
{
    public class Order
    {
        [Key]
        public long Id { get; set; }
        
        public DateTime OrderCreated { get; set; }
        
        public string OrderNumber { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }
        
        public int TotalWeight { get; set; }

        [ForeignKey("CustomerId")]
        public long CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        [ForeignKey("SellerId")]
        public long SellerId { get; set; }

        public virtual User Seller { get; set; }

        public long? ShipmentId { get; set; }

        public virtual Shipment Shipment { get; set; }

        [NotMapped]
        public OrderStatus OrderStatus { get; set; }

        [Column("OrderStatus")]
        public string OrderStatusString
        {
            get => OrderStatus.ToString();
            set => OrderStatus = (OrderStatus)(Enum.Parse(typeof(OrderStatus), value, true));
        }
        
        public virtual ICollection<OrderLineItem> Items { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public virtual ICollection<AudioRecord> AudioRecords { get; set; }
    }
}
