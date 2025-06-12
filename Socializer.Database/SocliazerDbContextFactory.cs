using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Socializer.Database;

public class SocializerDbContextFactory : IDesignTimeDbContextFactory<SocializerDbContext>
{
    public SocializerDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<SocializerDbContext>();
        var connectionString = config.GetConnectionString("SocializerConnectionString");

        optionsBuilder.UseSqlServer(connectionString);

        return new SocializerDbContext(optionsBuilder.Options);
    }
}