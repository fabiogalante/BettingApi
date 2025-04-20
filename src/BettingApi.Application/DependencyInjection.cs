using System.Reflection;
using BettingApi.Application.Common;
using BettingApi.Application.Common.Behaviors;
using BettingApi.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BettingApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        
        services.AddScoped<IMediator, Mediator>();
        
        services.Scan(scan =>
        {
            scan.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime(); // ou WithTransientLifetime()

            scan.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        });
        
        return services;
    }
}