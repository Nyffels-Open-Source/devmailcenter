using DevMailCenter.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Logic;

public interface ISmtpLogic
{
    void Send(SmtpSettings settings, Email email);
}

public class SmtpLogic : ISmtpLogic
{
    private readonly ILogger<EMailLogic> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    SmtpLogic(ILogger<EMailLogic> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public void Send(SmtpSettings settings, Email email)
    {
        // TODO
        return;
    }
}