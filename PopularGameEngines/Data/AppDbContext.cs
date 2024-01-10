using PopularGameEngines.Models;
using Microsoft.EntityFrameworkCore;

namespace PopularGameEngines.Data {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Message> Messages { get; set; }

        public DbSet<AppUser> Users { get; set; }
    }
}
