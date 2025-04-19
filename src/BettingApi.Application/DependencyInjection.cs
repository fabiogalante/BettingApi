// src/BettingApi.Application/DependencyInjection.cs

using System.Reflection;
using BettingApi.Application.Customers.Commands;
//using BettingApi.Application.Customers.Queries;
//using BettingApi.Application.Bets.Commands;
//using BettingApi.Application.Bets.Queries;
using BettingApi.Application.Common;
using BettingApi.Application.Common.Behaviors;
using BettingApi.Application.Common.Interfaces;
using BettingApi.Application.Customers.Query;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using ErrorOr;

namespace BettingApi.Application;

public static class DependencyInjection
{
    
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        
        services.AddScoped<IMediator, Mediator>();


// Register handlers
       // services.AddTransient<IQueryHandler<GetUserByIdQuery, UserDto?>, GetUserByIdQueryHandler>();
       // services.AddScoped<ICommandHandler<CreateCustomerCommand, string>, CreateCustomerCommandHandler>();
        //services.AddScoped<IQueryHandler<GetCustomerByIdQuery, string>, GetUserByIdQueryHandler>();
        
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
    
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
     
        
        
        
        // Registrar handlers manualmente
        //services.AddScoped<ICommandHandler<CreateCustomerCommand>, CreateCustomerCommandHandler>();
        //services.AddScoped<IQueryHandler<GetCustomersQuery, List<CustomerDto>>, GetCustomersQueryHandler>();
       // services.AddScoped<ICommandHandler<CreateBetCommand>, CreateBetCommandHandler>();
       // services.AddScoped<IQueryHandler<GetBetsQuery, List<BetDto>>, GetBetsQueryHandler>();
        
        // Registrar behaviors
      //  services.AddTransient(typeof(IPipelineBehavior<>), typeof(LoggingBehavior<>));
        
        // Registrar handlers no Dispatcher
        //Dispatcher.RegisterCommandHandler<CreateCustomerCommand, CreateCustomerCommandHandler>();
      //  Dispatcher.RegisterQueryHandler<GetCustomersQuery, List<CustomerDto>, GetCustomersQueryHandler>();
      //  Dispatcher.RegisterCommandHandler<CreateBetCommand, CreateBetCommandHandler>();
      //  Dispatcher.RegisterQueryHandler<GetBetsQuery, List<BetDto>, GetBetsQueryHandler>();
        
        return services;
    }
}