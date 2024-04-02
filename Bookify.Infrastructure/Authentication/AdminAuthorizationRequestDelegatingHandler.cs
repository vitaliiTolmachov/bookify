using System.Net.Http.Headers;
using System.Net.Http.Json;
using Bookify.Infrastructure.Authentication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.Authentication;

public sealed class AdminAuthorizationRequestDelegatingHandler : DelegatingHandler
{
    private readonly KeycloakOptions _keyCloakOptions;

    public AdminAuthorizationRequestDelegatingHandler(IOptions<KeycloakOptions> options)
    {
        _keyCloakOptions = options.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authToken = await GetAuthorizationTokenAsync(cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            authToken.AccessToken);

        var response = await base.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
        return response;
    }

    private async Task<BookifyAuthorizationToken> GetAuthorizationTokenAsync(CancellationToken cancellationToken)
    {
        var authorizationRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", _keyCloakOptions.AdminClientId),
            new("client_secret", _keyCloakOptions.AdminClientSecret),
            new("scope", "openid email"),
            new("grant_type", "client_credentials"),
        };

        var authorizationRequestContent = new FormUrlEncodedContent(authorizationRequestParameters);
        var authorizationRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_keyCloakOptions.TokenUrl))
        {
            Content = authorizationRequestContent
        };

        var authorizationResponse = await base.SendAsync(authorizationRequest, cancellationToken);
        authorizationResponse.EnsureSuccessStatusCode();

        return await authorizationResponse.Content.ReadFromJsonAsync<BookifyAuthorizationToken>(cancellationToken) ??
               throw new ApplicationException("Could not parse auth token");
    }
}