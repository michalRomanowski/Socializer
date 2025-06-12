using Microsoft.EntityFrameworkCore;
using Socializer.Database.Models;

namespace Socializer.Database
{
    public class SocializerDbContext(DbContextOptions<SocializerDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
