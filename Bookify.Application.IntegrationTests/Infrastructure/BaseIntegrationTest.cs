using Bookify.Infrastructure.Db;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Application.IntegrationTests.Infrastructure;

public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly ISender Mediator;
    protected readonly ApplicationDbContext DbContext;
    
    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        var scope = factory.Services.CreateScope();

        Mediator = scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
}