using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProdoctorovIntegration.Application.Options.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;
using AuthenticationOptions = ProdoctorovIntegration.Application.Options.Authentication.AuthenticationOptions;

namespace ProdoctorovIntegration.Application.Authentication;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private readonly AuthenticationOptions _authenticationOptions;

    public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IOptionsMonitor<AuthenticationOptions> authenticationOptions) : base(options, logger, encoder, clock)
    {
        _authenticationOptions = authenticationOptions.CurrentValue;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.ApiKeyHeader, out var apiKeyHeaderValues))
            return await Task.FromResult(AuthenticateResult.NoResult());

        var apiKeys = $"Token {_authenticationOptions.Token}";
        var providerApiKey = apiKeyHeaderValues.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(providerApiKey) || !apiKeys.Equals(providerApiKey))
            return await Task.FromResult(AuthenticateResult.Fail("Invalid Authentication Token"));

        var identity = new ClaimsIdentity(Array.Empty<Claim>(), ApiKeyAuthenticationOptions.AuthenticationType);
        var identities = new List<ClaimsIdentity> {identity};
        var principal = new ClaimsPrincipal(identities);
        var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationOptions.AuthenticationType);

        return await Task.FromResult(AuthenticateResult.Success(ticket));
    }
}