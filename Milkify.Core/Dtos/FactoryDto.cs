using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable;

namespace Milkify.Core.Dtos
{
    public class FactoryDto: BaseDto
    {
        public long Id { get; set; }

        [Required]
        [TableColumn("Name")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [TableColumn("PhoneNumber")]
        [DisplayName("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required]
        [TableColumn("Performance")]
        [DisplayName("Performance")]
        public int Performance { get; set; }

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