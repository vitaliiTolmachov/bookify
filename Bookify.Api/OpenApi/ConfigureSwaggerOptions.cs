using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public sealed class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _descriptionProvider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider descriptionProvider)
    {
        _descriptionProvider = descriptionProvider;
    }
    
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _descriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, GenerateApiInfo(description));
        }
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }
    
    private OpenApiInfo GenerateApiInfo(ApiVersionDescription versionDescription)
    {
        var openApiVersion = new OpenApiInfo
        {
            Title = $"Bookify.Api v{versionDescription.ApiVersion}",
            Version = versionDescription.ApiVersion.ToString()
        };

        if (versionDescription.IsDeprecated)
        {
            openApiVersion.Description += " This API version has been deprecated.";
        }

        return openApiVersion;
    }
}