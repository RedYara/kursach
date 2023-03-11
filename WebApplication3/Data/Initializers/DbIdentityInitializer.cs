using Microsoft.EntityFrameworkCore;
using WebApplication3.Persistence.Models;
using WebApplication3.Data;
using WebApplication3.Intrerfaces;
using WebApplication3.Models;

namespace WebApplication3.Data.Initializers
{
    public partial class DbIdentityInitializer
    {
        private static void RolesInitialize(_dbContext dbContext)
        {
            var rolesDbSet = dbContext.Roles;
            var roles = rolesDbSet.ToList();
            foreach (Roles role in Enum.GetValues(typeof(Roles)))
            {
                if (!roles.Any(x => x.Name == role.GetDescriptionValue()))
                {
                    rolesDbSet.Add(new Role { Name = role.GetDescriptionValue(), NormalizedName = role.GetDescriptionValue().ToUpperInvariant() });
                }
            }
            dbContext.SaveChanges();
        }
    }

}
