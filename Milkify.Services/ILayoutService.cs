using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Milkify.Core.Attributes;
using Milkify.Core.Entities.Membership;
using Milkify.Core.IoC;
using Milkify.Core.Models;

namespace Milkify.Services
{
    public interface ILayoutService : IDependency
    {
        IEnumerable<LayoutTabModel> GetHeaderTabs(ClaimsPrincipal currentUser);
    }

    public class LayoutService : ILayoutService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public LayoutService(UserManager<User> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        public IEnumerable<LayoutTabModel> GetHeaderTabs(ClaimsPrincipal currentUser)
        {
            var controllers = AppDomain.CurrentDomain.GetAssemblies().GetDefinedConcreteTypes()
                .Where(type => !type.IsGenericType)
                .Where(type => type.GetCustomAttribute<LayoutLink>() != null);
            List<string> currentUserRoles = _userService.GetRoles(_userService.GetId(currentUser)).ToList();

            var tabs = new List<LayoutTabModel>();

            foreach (var controller in controllers)
            {
                var attr = controller.GetCustomAttribute<LayoutLink>();
                if (string.IsNullOrEmpty(attr.Roles) || attr.Roles.Split(',').Any(currentUser.IsInRole))
                {
                    tabs.Add(new LayoutTabModel()
                    {
                        ActionName = attr.Action ?? "Index",
                        ControllerName = controller.Name.Replace("Controller", ""),
                        TabName = attr.Name ?? controller.Name.Replace("Controller", ""),
                        Order = attr.Order,
                        NavIcon = attr.NavIcon
                    });
                }
            }

            return tabs.OrderBy(x => x.Order);
        }
    }
}