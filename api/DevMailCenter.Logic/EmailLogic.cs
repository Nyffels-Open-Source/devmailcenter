using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Logic;

public interface IEmailLogic
{
    Guid Send(Guid emailId);
}

public class EmailLogic : IEmailLogic
{
    private readonly ILogger<EmailLogic> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public EmailLogic(ILogger<EmailLogic> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Guid Send(Guid emailId)
    {
        // TODO
        return Guid.NewGuid();
    }
}