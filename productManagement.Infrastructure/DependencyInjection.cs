using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using productManagement.Domain.Interfaces;
using productManagement.Infrastructure.Persistence;
using productManagement.Infrastructure.Repositories;

namespace productManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")??
                               Environment.GetEnvironmentVariable("SQL_CONNECTION");
        services.AddDbContext<AppDbContext>(options => 
            options.UseMySql(connectionString
            , ServerVersion.AutoDetect(connectionString)));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        return services;
    }
}