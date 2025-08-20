using Microsoft.EntityFrameworkCore;
using Socializer.Database.Models;

namespace Socializer.Database;

public class SocializerDbContext(DbContextOptions<SocializerDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Preference> Preferences { get; set; }
    public DbSet<UserPreference> UserPreferences { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatUser> ChatUsers { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("soc");

        modelBuilder.Entity<UserPreference>()
            .HasOne(up => up.User)
            .WithMany(u => u.UserPreferences)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserPreference>()
            .HasOne(up => up.Preference)
            .WithMany(p => p.UserPreferences)
            .HasForeignKey(up => up.PreferenceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserPreference>()
            .HasIndex(up => new { up.UserId, up.PreferenceId })
            .IsUnique();


        modelBuilder.Entity<ChatUser>()
            .HasOne(cu => cu.User)
            .WithMany(u => u.ChatUsers)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatUser>()
            .HasOne(cu => cu.Chat)
            .WithMany(c => c.ChatUsers)
            .HasForeignKey(cu => cu.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ChatUser>()
            .HasIndex(cu => new { cu.ChatId, cu.UserId })
            .IsUnique();
    }
}
