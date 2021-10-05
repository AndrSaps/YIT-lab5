using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable;

namespace Milkify.Core.Dtos
{
    public class UserDto : BaseDto
    {
        public long Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [DisplayName("Email")]
        [TableColumn("Email")]
        public string Email { get; set; }

        [Required]
        [TableColumn("First Name")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [TableColumn("Last Name")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Role")]
        [TableColumn("Role")]
        [RequireUserRoles("Admin, Seller")]
        public string UserRole { get; set; }

        public string CurrentPassword { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [DisplayName("Password")]
        public string NewPassword { get; set; }
        
        public string DisplayName => $"{FirstName} {LastName}";

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