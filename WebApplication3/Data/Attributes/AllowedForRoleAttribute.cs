using System.Collections.Generic;
using System.Linq;
using WebApplication3.Models;

namespace WebApplication3.Data.Initializers
{
    public class AllowedForRoleAttribute
    {
        private Role[] _roles;
        public List<Role> Roles { get { return _roles != null ? _roles.ToList() : new List<Role>(); } }
        public AllowedForRoleAttribute(params Role[] roles)
        {
            _roles = roles;
        }
    }
}
