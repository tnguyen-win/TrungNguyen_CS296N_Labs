using Microsoft.AspNetCore.Identity;

namespace PopularGameEngines.Models
{
    public class UserVM
    {
        private IEnumerable<AppUser> _users = new List<AppUser>();

        private IEnumerable<IdentityRole> _roles = new List<IdentityRole>();

        public IEnumerable<AppUser> Users { get => _users; set => _users = value; }

        public IEnumerable<IdentityRole> Roles { get => _roles; set => _roles = value; }
    }
}
