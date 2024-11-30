using DevMailCenter.Models;
using DevMailCenter.Repository;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Logic;

public interface IEmailLogic
{
    Guid Send(Guid emailId);
}

public class EmailLogic : IEmailLogic
{
    private readonly ILogger<EmailLogic> _logger;
    private readonly IEmailRepository _emailRepository;
    private readonly IMicrosoftLogic _microsoftLogic;
    private readonly ISmtpLogic _smtpLogic;
    private readonly IMailServerRepository _mailServerRepository;

    public EmailLogic(ILogger<EmailLogic> logger, IEmailRepository emailRepository, ISmtpLogic smtpLogic, IMailServerRepository mailServerRepository, IMicrosoftLogic microsoftLogic)
    {
        _logger = logger;
        _emailRepository = emailRepository;
        _smtpLogic = smtpLogic;
        _mailServerRepository = mailServerRepository;
        _microsoftLogic = microsoftLogic;
    }

    public Guid Send(Guid emailId)
    {
        var email = _emailRepository.Get(emailId, true);

        if (email is null)
        {
            throw new Exception("Email not found");
        }

        if (email.Status != EmailStatus.Concept)
        {
            throw new Exception("Email is not in 'concept' status.");
        }

        if (email.Receivers == null || email.Receivers.Count <= 0)
        {
            throw new Exception("Receivers are required to send the e-mail");
        }

        var server = _mailServerRepository.Get(email.ServerId, true, true);
        return server.Type switch
        {
            MailServerType.Smtp => _smtpLogic.Send(GetSmtpSettingsFromMailServer(server), email),
            MailServerType.MicrosoftExchange => _microsoftLogic.Send(GetMicrosoftSettingsFromMailServer(server), email),
            _ => throw new NotImplementedException("Sending email is not implemented yet for this mail server type.")
        };
    }

    private SmtpSettings GetSmtpSettingsFromMailServer(MailServer mailServer)
    {
        return new SmtpSettings
        {
            ssl = mailServer.MailServerSettings.First(e => e.Key == "ssl").Value.ToLower() == "true",
            Email = mailServer.MailServerSettings.First(e => e.Key == "email").Value,
            Host = mailServer.MailServerSettings.First(e => e.Key == "host").Value,
            Password = mailServer.MailServerSettings.First(e => e.Key == "password").Value,
            Port = int.Parse(mailServer.MailServerSettings.First(e => e.Key == "port").Value),
            User = mailServer.MailServerSettings.First(e => e.Key == "user").Value,
            Name = mailServer.MailServerSettings.First(e => e.Key == "username").Value
        };
    }

    private MicrosoftSettings GetMicrosoftSettingsFromMailServer(MailServer mailServer)
    {
        return new MicrosoftSettings
        {
            RefreshToken = mailServer.MailServerSettings.First(e => e.Key == "refreshToken").Value,
        };
    }
}