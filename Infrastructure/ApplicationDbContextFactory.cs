using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AuctionSystem.Infrastructure;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // محاولة العثور على appsettings.json في مشروع العرض (presentation) أولاً
        string basePath = Directory.GetCurrentDirectory();
        string presentationPath = Path.Combine(basePath, "../presentation");

        // إذا كنا في مجلد infrastructure ووجدنا مجلد presentation يحتوي على appsettings.json
        if (Directory.Exists(presentationPath) && File.Exists(Path.Combine(presentationPath, "appsettings.json")))
        {
            basePath = presentationPath; // استخدم مسار المشروع الرئيسي للـ configuration
        }

        // بناء الإعدادات من ملفات json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        // قراءة سلسلة الاتصال
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // إذا لم توجد سلسلة الاتصال في الملفات، نستخدم fallback
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = "Server=9F41;Database=AuctionSystemDb;User Id=loan;Password=1234;TrustServerCertificate=True;";
        }

        // استخدام SQL Server
        optionsBuilder.UseSqlServer(connectionString);

        // إعادة DbContext جاهز للاستخدام مع Migrations
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
