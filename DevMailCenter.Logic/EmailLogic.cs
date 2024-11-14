using DevMailCenter.Models;
using DevMailCenter.Repository;
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
        var email = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailRepository>()
            .Get(emailId, true);
        if (email is null)
        {
            throw new Exception("Email not found");
        }

        if (email.Receivers == null || email.Receivers.Count <= 0)
        {
            throw new Exception("Receivers are required to send the e-mail");
        }

        var server = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IMailServerRepository>()
            .Get(email.ServerId);
        return server.Type switch
        {
            MailServerType.Smtp => _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ISmtpLogic>().Send(GetSmtpSettingsFromMailServer(server), email),
            MailServerType.MicrosoftExchange => throw new NotImplementedException("Sending email is not implemented yet for this mail server type."),
            _ => throw new NotImplementedException("Sending email is not implemented yet for this mail server type.")
        };
    }

    private SmtpSettings GetSmtpSettingsFromMailServer(MailServer mailServer)
    {
        return new SmtpSettings
        {
            ssl = mailServer.MailServerSettings.First(e => e.Key == "ssl").Value == "true",
            Email = mailServer.MailServerSettings.First(e => e.Key == "email").Value,
            Host = mailServer.MailServerSettings.First(e => e.Key == "host").Value,
            Password = mailServer.MailServerSettings.First(e => e.Key == "password").Value,
            Port = int.Parse(mailServer.MailServerSettings.First(e => e.Key == "port").Value),
            User = mailServer.MailServerSettings.First(e => e.Key == "user").Value
        };
    }
}