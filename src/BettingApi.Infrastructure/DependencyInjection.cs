using BettingApi.Core.Bets;
using BettingApi.Core.Common.Interfaces;
using BettingApi.Core.Customers;
using BettingApi.Infrastructure.Data;
using BettingApi.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BettingApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("BettingDb"));
        
        // Registrar repositório genérico
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        // Registrar repositórios específicos se necessário
        services.AddScoped<IRepository<Customer>, Repository<Customer>>();
        services.AddScoped<IRepository<Bet>, Repository<Bet>>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
