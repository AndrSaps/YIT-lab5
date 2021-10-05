using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Milkify.Core.Entities.Membership;
using Milkify.Core.Enums;

namespace Milkify.Core.Entities
{
    public class Shipment
    {
        [Key]
        public long Id { get; set; }

        public string ShipmentNumber { get; set; }

        public DateTime? ShippedDate { get; set; }

        public DateTime? DeliveredDate { get; set; }

        public virtual Location ShipmentLocation { get; set; }
        
        public long OrderId { get; set; }

        public virtual Order Order { get; set; }
        
        public long? RouteId { get; set; }

        public virtual Route Route { get; set; }

        [NotMapped]
        public ShipmentStatus ShipmentStatus { get; set; }

        [Column("ShipmentStatus")]
        public string ShipmentStatusString
        {
            get => ShipmentStatus.ToString();
            set => ShipmentStatus = (ShipmentStatus)(Enum.Parse(typeof(ShipmentStatus), value, true));
        }
    }
}
