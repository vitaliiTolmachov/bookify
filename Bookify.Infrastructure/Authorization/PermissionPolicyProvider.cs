using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.Authorization;

internal sealed class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _authOptions;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    {
        _authOptions = options.Value;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy is not null)
        {
            return policy;
        }

        var permissionPolicy = new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionAuthRequirement(policyName))
            .Build();
        
        //Caching
        _authOptions.AddPolicy(policyName, permissionPolicy);

        return permissionPolicy;
    }
}