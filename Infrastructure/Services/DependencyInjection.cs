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
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<IAuctionRepository, AuctionRepository>();
            services.AddScoped<IBidRepository, BidRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVerficationDocRepository, VerificationDocumentRepository>();

            // Identity Services (custom authentication / registration logic if needed)
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();

            // General Infrastructure Services
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }
    }
}
