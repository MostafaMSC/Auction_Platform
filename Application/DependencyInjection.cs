using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using AuctionSystem.Application.Common.Behaviors;

namespace AuctionSystem.Application.Validation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
{
    services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

    // Use typeof to ensure correct assembly
    services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    return services;
}
    }
}
