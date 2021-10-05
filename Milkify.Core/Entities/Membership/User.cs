using Microsoft.AspNetCore.Identity;

namespace Milkify.Core.Entities.Membership
{
    public class User : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
