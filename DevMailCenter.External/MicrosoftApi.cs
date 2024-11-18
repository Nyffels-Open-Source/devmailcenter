using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.External;

public interface IMicrosoftApi
{
    Task<string> GetRefreshTokenByAuthorizationCode(string authorizationCode);
}

public class MicrosoftApi : IMicrosoftApi
{
    private readonly ILogger<MicrosoftApi> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MicrosoftApi(ILogger<MicrosoftApi> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task<string> GetRefreshTokenByAuthorizationCode(string authorizationCode)
    {
        return Task.FromResult("OK");
    }
}