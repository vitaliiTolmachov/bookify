using Bookify.Application.Behaviours;
using Bookify.Domain.Bookings;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            
            //Order is important!
            config.AddOpenBehavior(typeof(LoggingCommandBehaviour<,>));
            config.AddOpenBehavior(typeof(ValidatingCommandBehaviour<,>));
            config.AddOpenBehavior(typeof(QueryCachingBehaviour<,>));
        });
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        services.AddTransient<PricingService>();
        return services;
    }
}