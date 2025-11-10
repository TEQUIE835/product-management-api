using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using productManagement.Application.Interfaces.Auth;
using productManagement.Application.Interfaces.Products;
using productManagement.Application.Interfaces.Security;
using productManagement.Application.Interfaces.Users;
using productManagement.Application.Services.Auth;
using productManagement.Application.Services.Products;
using productManagement.Application.Services.Security;
using productManagement.Application.Services.Users;

namespace productManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IUserService, UserService>();
        AddJwtAuthentication(services, configuration);

        return services;
    }

    private static void AddJwtAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey =
            configuration["Jwt:Key"] ??
            configuration["SecretKey"] ??
            Environment.GetEnvironmentVariable("SECRET_KEY");

        var jwtIssuer =
            configuration["Jwt:Issuer"] ??
            configuration["Issuer"] ??
            Environment.GetEnvironmentVariable("ISSUER");

        var jwtAudience =
            configuration["Jwt:Audience"] ??
            configuration["Audience"] ??
            Environment.GetEnvironmentVariable("AUDIENCE");

        if (string.IsNullOrWhiteSpace(jwtKey))
            throw new InvalidOperationException("JWT Key is not configured. Set Jwt:Key or SECRET_KEY.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = !string.IsNullOrWhiteSpace(jwtIssuer),
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = !string.IsNullOrWhiteSpace(jwtAudience),
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });
    }
}