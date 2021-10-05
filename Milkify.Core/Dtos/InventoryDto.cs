using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable;

namespace Milkify.Core.Dtos
{
    public class InventoryDto: BaseDto
    {
        public long Id { get; set; }

        [Required]
        [TableColumn("Shelf")]
        [DisplayName("Shelf")]
        public string Shelf { get; set; }

        [Required]
        [TableColumn("Supply Code")]
        public string SupplyCode { get; set; }

        [Required]
        [TableColumn("Quantity")]
        public string SupplyQuantity { get; set; }

        [Required]
        [TableColumn("Reserved Quantity")]
        public string ReservedSupplyQuantity { get; set; }

        [Required]
        [DisplayName("Product")]
        public long? ProductId { get; set; }

        [TableColumn("Product Name", "ProductName")]
        public ProductDto Product { get; set; }

        [Required]
        [DisplayName("Factory")]
        public long? FactoryId { get; set; }

        [TableColumn("Factory", "Name")]
        public FactoryDto Factory { get; set; }

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