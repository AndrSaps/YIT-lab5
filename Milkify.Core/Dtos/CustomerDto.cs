using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable;
using Milkify.Core.Entities;

namespace Milkify.Core.Dtos
{
    public class CustomerDto: BaseDto
    {
        public long Id { get; set; }

        [Required]
        [DisplayName("Name")]
        [TableColumn("Name")]
        public string Name { get; set; }

        [Required]
        [TableColumn("Phone Number")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public long LocationId { get; set; }

        [TableColumn("Address", "Address")]
        public LocationDto Location { get; set; }

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