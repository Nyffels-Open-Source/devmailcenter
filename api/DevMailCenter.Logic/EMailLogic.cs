using DevMailCenter.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Logic;

public interface IMailServerLogic
{
    void SendRequest(Guid emailId, Guid serverId);
}

public class EMailLogic : IMailServerLogic
{
    private readonly ILogger<EMailLogic> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    EMailLogic(ILogger<EMailLogic> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public void SendRequest(Guid emailId, Guid serverId)
    {
        // TODO
        return;
    }
}