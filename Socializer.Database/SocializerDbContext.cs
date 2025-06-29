using Microsoft.EntityFrameworkCore;
using Socializer.Database.Models;

namespace Socializer.Database
{
    public class SocializerDbContext(DbContextOptions<SocializerDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Preference> Preferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Preference>()
                .HasOne(d => d.User)
                .WithMany(u => u.Preferences)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
