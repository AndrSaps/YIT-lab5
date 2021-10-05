using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable.DatatableRequests;
using Milkify.Core.Dtos;
using Milkify.Core.Entities.Membership;
using Milkify.Services;

namespace Milkify.Controllers
{
    [LayoutLink(order:1, Roles = "Admin")]
    public class UserController : BaseCrudController<User, UserDto, IUserService, DefaultDataTablesRequest>
    {
        private readonly IUserRoleService _userRoleService;

        public UserController(IUserService userService, IUserRoleService userRoleService) : base(userService)
        {
            _userRoleService = userRoleService;
        }

        protected override void PopulateNeededInfo(UserDto dto = null, DefaultDataTablesRequest request = null)
        {
            PopulateUserRoleDropDownList();
        }

        private void PopulateUserRoleDropDownList(object userRole = null)
        {
            var userQuery = _userRoleService.GetAllDtos();
            ViewBag.UserRoles = new SelectList(userQuery, "Name", "Name", userRole);
        }
    }
}