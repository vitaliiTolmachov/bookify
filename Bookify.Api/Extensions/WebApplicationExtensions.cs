using Bookify.Api.Middleware;
using Bookify.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ApplyMigrations(this WebApplication appBuilder)
    {
        using var scope = appBuilder.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();

        return appBuilder;
    }
    
    public static WebApplication UseCustomGlobalExceptionMiddleware(this WebApplication appBuilder)
    {
        appBuilder.UseMiddleware<ExceptionHandlingMiddleware>();

        return appBuilder;
    }
}