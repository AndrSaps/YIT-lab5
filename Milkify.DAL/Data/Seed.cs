using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Milkify.Core.Entities.Membership;
using Milkify.Core.Enums;

namespace Milkify.DAL.Data
{
    public static class Seed
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            // adding customs roles
            var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            string[] roleNames = { UserRoles.Admin.ToString(), UserRoles.Seller.ToString(), UserRoles.Driver.ToString() };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new UserRole(roleName));
                }
            }

            var admin = new User
            {
                UserName = configuration.GetSection("UserSettings")["Email"],
                Email = configuration.GetSection("UserSettings")["Email"]
            };
            string userPassword = configuration.GetSection("UserSettings")["UserPassword"];
            var user = await userManager.FindByEmailAsync(configuration.GetSection("UserSettings")["Email"]);
            if (user == null)
            {
                var createAdminUser = await userManager.CreateAsync(admin, userPassword);
                if (createAdminUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, UserRoles.Admin.ToString());
                }
            }
        }
    }
}
