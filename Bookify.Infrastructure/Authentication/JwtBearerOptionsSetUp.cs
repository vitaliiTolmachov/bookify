using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.Authentication;

public class JwtBearerOptionsSetUp : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly AuthenticationOptions _options;

    public JwtBearerOptionsSetUp(IOptions<AuthenticationOptions> options)
    {
        _options = options.Value;
    }
    
    public void Configure(JwtBearerOptions options)
    {
        options.Audience = _options.Audience;
        options.MetadataAddress = _options.MetadataUrl;
        options.TokenValidationParameters.ValidIssuer = _options.ValidIssuer;
        options.RequireHttpsMetadata = _options.RequireHttpsMetadata;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
}