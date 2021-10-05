using System;
using System.Collections.Generic;
using System.Security.Claims;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable;
using Milkify.Core.Enums;

namespace Milkify.Core.Dtos
{
    public class ShipmentDto : BaseDto
    {
        public long Id { get; set; }

        [TableColumn("Shipment Number")]
        public string ShipmentNumber { get; set; }

        [TableColumn("Shipped Date")]
        public DateTime? ShippedDate { get; set; }

        [TableColumn("Delivered Date")]
        public DateTime? DeliveredDate { get; set; }
       
        public long OrderId { get; set; }

        [TableColumn("Order Number", nameof(OrderDto.OrderNumber))]
        public OrderDto Order { get; set; }
        
        public LocationDto ShipmentLocation { get; set; }

        public ShipmentStatus ShipmentStatus { get; set; }

        public long? RouteId { get; set; }

        public RouteDto Route { get; set; }

        public override IList<DatatableAction> GetDtoActions(ClaimsPrincipal currentUser)
        {
            var actions = new List<DatatableAction>();
            if (currentUser.IsInRole("Admin") || currentUser.IsInRole("Driver"))
            {
                actions.Add(new DatatableAction()
                {
                    Name = "Edit",
                    CssIconClass = "fa fa-edit"
                });

                if (currentUser.IsInRole("Admin"))
                {
                    actions.Add(new DatatableAction()
                    {
                        Name = "Delete",
                        CssIconClass = "fa fa-trash"
                    });
                }

            }
            return actions;
        }
    }
}