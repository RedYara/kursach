using Microsoft.AspNetCore.Identity;

namespace WebApplication3.Models
{
    public class UserRoles : IdentityUserRole<string>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

    }
}