using DevMailCenter.Models;
using DevMailCenter.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;

namespace DevMailCenter.Logic;

public interface ISmtpLogic
{
    Guid Send(SmtpSettings settings, Email email);
}

public class SmtpLogic : ISmtpLogic
{
    private readonly ILogger<EmailLogic> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SmtpLogic(ILogger<EmailLogic> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Guid Send(SmtpSettings settings, Email email)
    {
        var emailRepository = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailRepository>();
        var emailTransactionRepository = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailTransactionRepository>();
        
        var mm = new MimeMessage();

        mm.From.Add(new MailboxAddress(settings.Name, settings.Email));
        foreach (var receiver in email.Receivers)
        {
            switch (receiver.Type)
            {
                case EmailReceiverType.To:
                    mm.To.Add(new MailboxAddress(receiver.ReceiverName, receiver.ReceiverEmail)); 
                    break;
                case EmailReceiverType.CC:
                    mm.Cc.Add(new MailboxAddress(receiver.ReceiverName, receiver.ReceiverEmail));
                    break;
                case EmailReceiverType.BCC:
                    mm.Bcc.Add(new MailboxAddress(receiver.ReceiverName, receiver.ReceiverEmail));
                    break;
            }
        }

        mm.Subject = email.Subject;
        mm.Body = new TextPart(MimeKit.Text.TextFormat.Html) { 
            Text = email.Message
        };

        try
        {
            emailRepository.SetEmailStatus(email.Id, EmailStatus.Pending);
            
            var client = new SmtpClient();
            client.Connect(settings.Host, settings.Port, settings.ssl);
            client.Authenticate(settings.User, settings.Password);
            
            var rawServerResponse = client.Send(mm);
            client.Disconnect(true);
     
            emailRepository.SetEmailStatus(email.Id, EmailStatus.Sent);
            return emailTransactionRepository.Create(email.Id, rawServerResponse);
        }
        catch (Exception ex)
        {
            emailRepository.SetEmailStatus(email.Id, EmailStatus.Failed);
            return emailTransactionRepository.Create(email.Id, ex.Message);
        }
    }
}