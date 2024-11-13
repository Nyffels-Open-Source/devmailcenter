using DevMailCenter.Models;
using DevMailCenter.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Logic;

public interface IMailServerLogic
{
    Guid CreateMailServer(MailServerCreate mailServer);
    void UpdateMailServer(Guid id, MailServerUpdate mailServer);
}

public class MailServerLogic : IMailServerLogic
{
    private readonly ILogger<IMailServerLogic> _logger;
    public readonly IServiceScopeFactory _serviceScopeFactory;

    public MailServerLogic(ILogger<MailServerLogic> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Guid CreateMailServer(MailServerCreate mailServer)
    {
        var mailServerRepository = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IMailServerRepository>();
        var duplicateMailService = mailServerRepository.GetByName(mailServer.Name);
        if (duplicateMailService != null)
        {
            throw new Exception("Duplicate mail server name");
        }

        return mailServerRepository.Create(mailServer);
    }
}