using System.Net.Http.Json;
using Bookify.Application.Abstractions.Authentication;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Authentication.Models;

namespace Bookify.Infrastructure.Authentication;

public sealed class BookifyAuthenticationService : IBookifyAuthenticationService
{
    private const string PasswordCredentialType = "password";
    private readonly HttpClient _httpClient;

    public BookifyAuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<string> AuthenticateAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        var userRepresentationModel = UserRepresentationModel.FromUser(user);
        
        userRepresentationModel.Credentials = new UserCredentialsRepresentationModel[]
        {
            //KeyCloak sign in request
            new()
            {
                Value = password,
                Temporary = false,
                Type = PasswordCredentialType
            }
        };
        
        var response = await _httpClient.PostAsJsonAsync(
            "users",
            userRepresentationModel,
            cancellationToken);
        
        return ExtractIdentityIdFromLocationHeader(response);
    }

    //Extracts identityId located in response query all after segment users/
    private string ExtractIdentityIdFromLocationHeader(HttpResponseMessage response)
    {
        const string userSegment = "users/";
        var locationHeader = response.Headers.Location?.PathAndQuery;

        if (string.IsNullOrEmpty(locationHeader))
            throw new InvalidOperationException("Location header can't be empty");

        var userSegmentIndex =
            locationHeader.IndexOf(userSegment, StringComparison.InvariantCultureIgnoreCase);

        var identityId = locationHeader.Substring(userSegmentIndex + userSegment.Length);

        return identityId;

    }
}