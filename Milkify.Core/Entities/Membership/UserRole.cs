using Microsoft.AspNetCore.Identity;

namespace Milkify.Core.Entities.Membership
{
    public class UserRole : IdentityRole<long>
    {
        public UserRole() : base()
        {
        }
        public UserRole(string roleName) : base (roleName)
        {
        }
    }
}