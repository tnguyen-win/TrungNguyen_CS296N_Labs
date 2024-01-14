using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PopularGameEngines.Models
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
