using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable;

namespace Milkify.Core.Dtos
{
    public class TruckDto : BaseDto
    {
        public long Id { get; set; }

        [Required]
        [TableColumn("Serial Number")]
        public string SerialNumber { get; set; }

        [Required]
        [TableColumn("Lifting Weight")]
        public int LiftingWeight { get; set; }
        public override IList<DatatableAction> GetDtoActions(ClaimsPrincipal currentUser)
        {
            var actions = new List<DatatableAction>();
            if (currentUser.IsInRole("Admin") || currentUser.IsInRole("Seller"))
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