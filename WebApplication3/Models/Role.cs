using Microsoft.AspNetCore.Identity;

namespace WebApplication3.Models
{
    public class Role : IdentityRole
    {
        public ICollection<UserRoles> UserRoles { get; set; }
    }
}
