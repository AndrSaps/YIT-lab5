using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Milkify.Core.Entities.Membership;

namespace Milkify.Core.Entities
{
    public class Route
    {
        [Key]
        public long Id { get; set; }

        public DateTime? RouteStarted { get; set; }

        public DateTime? RouteFinished { get; set; }

        public long TruckId { get; set; }

        public virtual Truck Truck { get; set; }

        public long DriverId { get; set; }

        public virtual User Driver { get; set; }
        
        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}