using Microsoft.AspNetCore.Authentication;

namespace ProdoctorovIntegration.Application.Options.Authentication;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string ApiKeyHeader = "Authorization";
    public const string AuthenticationScheme = "Bearer";
    public static string AuthenticationType => AuthenticationScheme;
}