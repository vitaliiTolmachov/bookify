using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Caching;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Email;
using Bookify.Application.Data;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Authentication;
using Bookify.Infrastructure.Authorization;
using Bookify.Infrastructure.Caching;
using Bookify.Infrastructure.Clock;
using Bookify.Infrastructure.Db;
using Bookify.Infrastructure.Db.Repositories;
using Bookify.Infrastructure.Email;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using AuthenticationOptions = Bookify.Infrastructure.Authentication.AuthenticationOptions;

namespace Bookify.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEmailService, EmailService>();

        AddDatabase(services, configuration);

        ConfigureAuthentication(services, configuration);

        ConfigureAuthorization(services);

        AddCaching(services, configuration);

        return services;
    }

    private static IServiceCollection ConfigureAuthorization(
        IServiceCollection services)
    {
        //Role-based authorization
        services.AddScoped<RolesProvider>();
        services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();
        
        //Permission-based authorization
        services.AddScoped<PermissionsProvider>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddTransient<IAuthorizationHandler, PermissionAuthHandler>();
        
        return services;
    }

    private static IServiceCollection ConfigureAuthentication(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters.ValidateActor = false;
                options.TokenValidationParameters.ValidateAudience = false;
                options.TokenValidationParameters.ValidateIssuerSigningKey = false;
                options.TokenValidationParameters.SignatureValidator = (token, _) => new JsonWebToken(token);
            });
        
        services.Configure<AuthenticationOptions>(configuration.GetSection("AuthenticationOptions") ??
                                                  throw new KeyNotFoundException("AuthenticationOptions"));
        
        services.ConfigureOptions<JwtBearerOptionsSetUp>();

        services.Configure<KeycloakOptions>(configuration.GetSection("KeyCloak") ??
                                            throw new KeyNotFoundException("KeyCloak"));
        services.AddTransient<AdminAuthorizationRequestDelegatingHandler>();
        services.AddHttpClient<IBookifyAuthenticationService, BookifyAuthenticationService>((provider, client) =>
        {
            var keycloakOptions = provider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
            client.BaseAddress = new Uri(keycloakOptions.AdminUrl);
        }).AddHttpMessageHandler<AdminAuthorizationRequestDelegatingHandler>();

        services.AddHttpClient<IJwtService, JwtService>((provider, client) =>
        {
            var keycloakOptions = provider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
            client.BaseAddress = new Uri(keycloakOptions.TokenUrl);
        });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserIdentityProvider, UserIdentityProvider>();

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ??
                               throw new KeyNotFoundException("Database");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IApartmentRepository, ApartmentRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<IDbConnectionFactory>(new DbConnectionFactory(connectionString));
        
        SqlMapper.AddTypeHandler(new DateOnlyHandler());
            
        return services;
    }

    private static IServiceCollection AddCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Cache") ??
                               throw new KeyNotFoundException("Cache");

        services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);
        services.AddSingleton<ICacheService, CacheService>();
        return services;
    }
}