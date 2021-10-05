using System;
using System.Collections.Generic;
using System.Security.Claims;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable;
using Milkify.Core.Entities.Membership;

namespace Milkify.Core.Dtos
{
    public class RouteDto : BaseDto
    {
        public long Id { get; set; }

        [TableColumn("Started")]
        public DateTime? RouteStarted { get; set; }

        [TableColumn("Finished")]
        public DateTime? RouteFinished { get; set; }

        public long TruckId { get; set; }

        [TableColumn("Truck Serial Number", nameof(TruckDto.SerialNumber))]
        public TruckDto Truck { get; set; }

        public long DriverId { get; set; }

        [TableColumn("Driver", nameof(UserDto.DisplayName))]
        public UserDto Driver { get; set; }

        public List<ShipmentDto> Shipments { get; set; }

        public bool IsEditable => !RouteStarted.HasValue;

        public override IList<DatatableAction> GetDtoActions(ClaimsPrincipal currentUser)
        {
            var actions = new List<DatatableAction>();
            if (currentUser.IsInRole("Admin"))
            {
                actions.Add(new DatatableAction()
                {
                    Name = "Edit",
                    CssIconClass = "fa fa-edit"
                });
                actions.Add(new DatatableAction()
                {
                    Name = "Delete",
                    CssIconClass = "fa fa-trash"
                });
            }
            return actions;
        }
    }
}