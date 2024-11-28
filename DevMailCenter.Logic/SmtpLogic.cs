using DevMailCenter.Models;
using DevMailCenter.Repository;
using DevMailCenter.Security;
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
    private readonly IEmailRepository _emailRepository;
    private readonly IEmailTransactionRepository _emailTransactionRepository;
    private readonly IEncryptionLogic _encryptionLogic;

    public SmtpLogic(ILogger<EmailLogic> logger, IEmailRepository emailRepository, IEmailTransactionRepository emailTransactionRepository, IEncryptionLogic encryptionLogic)
    {
        _logger = logger;
        _emailRepository = emailRepository;
        _emailTransactionRepository = emailTransactionRepository;
        _encryptionLogic = encryptionLogic;
    }

    public Guid Send(SmtpSettings settings, Email email)
    {
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
            _emailRepository.SetEmailStatus(email.Id, EmailStatus.Pending);
            
            var client = new SmtpClient();
            client.Connect(settings.Host, settings.Port, settings.ssl);
            client.Authenticate(settings.User, _encryptionLogic.Decrypt(settings.Password));
            
            var rawServerResponse = client.Send(mm);
            client.Disconnect(true);
     
            _emailRepository.SetEmailStatus(email.Id, EmailStatus.Sent);
            return _emailTransactionRepository.Create(email.Id, rawServerResponse);
        }
        catch (Exception ex)
        {
            _emailRepository.SetEmailStatus(email.Id, EmailStatus.Failed);
            return _emailTransactionRepository.Create(email.Id, ex.Message);
        }
    }
}