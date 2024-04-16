using Bookify.Api.Extensions;
using Bookify.Application;
using Bookify.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var versionDescriptors = app.DescribeApiVersions();
        foreach (var description in versionDescriptors)
        {
            var versionUrl = $"/swagger/{description.GroupName}/swagger.json";
            var versionName = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(versionUrl, versionName);

        }
    });
    app.ApplyMigrations();
    //app.SeedData();
}

app.UseHttpsRedirection();

app.UseCorrelationIdMiddleware();
app.UseSerilogRequestLogging();

app.UseCustomGlobalExceptionMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

public partial class BookifyStartup;