using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable;

namespace Milkify.Core.Dtos
{
    public class ProductDto: BaseDto
    {
        public long Id { get; set; }

        [Required]
        [TableColumn("Name", order: -2)]
        [DisplayName("Name")]
        public string ProductName { get; set; }

        [Required]
        [Range(minimum:0, maximum:double.MaxValue, ErrorMessage = "Price should greater than zero")]
        [TableColumn("Price")]
        [DisplayName("Price")]
        public decimal? Price { get; set; }

        [Required]
        [Range(minimum: 0, maximum: double.MaxValue, ErrorMessage = "Weight should greater than zero")]
        [TableColumn("Weight")]
        [DisplayName("Weight")]
        public decimal? Weight { get; set; }

        [Required]
        [DisplayName("Category")]
        public long? CategoryId { get; set; }

        [TableColumn("Category", "Name", -1)]
        public CategoryDto Category { get; set; }

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