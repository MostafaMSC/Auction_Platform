using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AuctionSystem.Infrastructure;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Try to find appsettings.json in presentation project first
        string basePath = Directory.GetCurrentDirectory();
        string presentationPath = Path.Combine(basePath, "../presentation");
        
        // Check if we're in infrastructure directory and presentation exists
        if (Directory.Exists(presentationPath) && File.Exists(Path.Combine(presentationPath, "appsettings.json")))
        {
            basePath = presentationPath;
        }
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            // Fallback connection string if not found in config
            connectionString = "Server=9F41;Database=AuctionSystemDb;User Id=loan;Password=1234;TrustServerCertificate=True;";
        }

        optionsBuilder.UseSqlServer(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}