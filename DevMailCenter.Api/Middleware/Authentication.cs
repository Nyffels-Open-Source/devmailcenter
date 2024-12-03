using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DevMailCenter.Api.Middleware;

public class DmcAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IConfiguration _configuration;
    
    public DmcAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder, 
        ISystemClock clock,
        IConfiguration configuration)
        : base(options, logger, encoder, clock)
    {
        _configuration = configuration;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var requiredUser = _configuration["Authentication:User"];
        var requiredPassword = _configuration["Authentication:Password"];
        
        if (_configuration["Authentication:Enabled"] != "True")
        {
            var disabledClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, requiredUser)
            };
            var disabledIdentity = new ClaimsIdentity(disabledClaims, "Basic");
            var disabledClaimsPrincipal = new ClaimsPrincipal(disabledIdentity);
            return AuthenticateResult.Success(new AuthenticationTicket(disabledClaimsPrincipal, Scheme.Name));
        }
        
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }
        
        string authorizationHeader = Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }
        
        if (!authorizationHeader.StartsWith("basic ", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }
        
        var token = authorizationHeader.Substring(6);
        var credentialAsString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        
        var credentials = credentialAsString.Split(":");
        if (credentials?.Length != 2)
        {
            return AuthenticateResult.Fail("Unauthorized");
        }
        
        var username = credentials[0];
        var password = credentials[1];
        
        if (username != requiredUser && password != requiredPassword)
        {
            return AuthenticateResult.Fail("Authentication failed");
        }
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, username)
        };
        var identity = new ClaimsIdentity(claims, "Basic");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
    }
}