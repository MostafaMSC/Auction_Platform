using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AuctionSystem.Infrastructure.Repositories;
using AuctionSystem.Domain.Repositories;
using AuctionSystem.Infrastructure.Services;
using AuctionSystem.Application.Interfaces;
using AuctionSystem.Application.Interfaces.Identity;
using AuctionSystem.Infrastructure.Identity;

namespace AuctionSystem.Infrastructure
{
    public static class DependencyInjection
{
    // امتداد لـ IServiceCollection لإضافة كل البنية التحتية (Infrastructure)
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // ===== إعداد قاعدة البيانات =====
        // ربط DbContext مع SQL Server باستخدام سلسلة الاتصال من appsettings.json
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // ===== تسجيل المستودعات (Repositories) =====
        // كل Repository يمثل واجهة للتعامل مع البيانات
        services.AddScoped<IAuctionRepository, AuctionRepository>();
        services.AddScoped<IBidRepository, BidRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVerficationDocRepository, VerificationDocumentRepository>();

        // ===== تسجيل خدمات الهوية (Identity / Authentication) =====
        // خدمات مخصصة للتسجيل، تسجيل الدخول، JWT token
        services.AddScoped<RoleService>(); // إدارة الأدوار
        services.AddScoped<IAuthService, AuthService>(); // الخدمة العامة للمصادقة
        services.AddScoped<IJwtTokenService, JwtTokenService>(); // إنشاء والتحقق من التوكن
        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>(); // تسجيل الدخول
        services.AddScoped<IUserRegistrationService, UserRegistrationService>(); // تسجيل مستخدم جديد

        // ===== خدمات عامة أخرى =====
        services.AddScoped<IFileStorageService, LocalFileStorageService>(); // تخزين الملفات
        services.AddScoped<INotificationService, NotificationService>(); // إشعارات المستخدمين

        return services; // إرجاع IServiceCollection لإمكانية السلسلة
    }
}

}
