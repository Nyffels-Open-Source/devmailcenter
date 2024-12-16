using DevMailCenter.Models;
using DevMailCenter.Repository;
using DevMailCenter.Security;
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
    private readonly IMailServerRepository _mailServerRepository;
    private readonly IEmailAttachmentRepository _emailAttachmentRepository;

    public SmtpLogic(ILogger<EmailLogic> logger, IEmailRepository emailRepository, IEmailTransactionRepository emailTransactionRepository, IEncryptionLogic encryptionLogic, IMailServerRepository mailServerRepository, IEmailAttachmentRepository emailAttachmentRepository)
    {
        _logger = logger;
        _emailRepository = emailRepository;
        _emailTransactionRepository = emailTransactionRepository;
        _encryptionLogic = encryptionLogic;
        _mailServerRepository = mailServerRepository;
        _emailAttachmentRepository = emailAttachmentRepository;
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
        
        var multipart = new Multipart("mixed");
        multipart.Add(new TextPart(MimeKit.Text.TextFormat.Html) 
        { 
            Text = email.Message
        });
        foreach (var att in email.Attachments)
        {
            var content = _emailAttachmentRepository.GetAttachmentStream(att.Id);
            if (content != null)
            {
                throw new Exception($"Attachment {att.Id} no longer exists in storage.");
            }
            
            var attachment = new MimePart (att.Mime) {
                Content = new MimeContent (content, ContentEncoding.Base64),
                ContentDisposition = new ContentDisposition (ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName (att.Name)
            };
            
            multipart.Add(attachment);
        }

        mm.Subject = email.Subject;
        mm.Body = multipart;

        try
        {
            _emailRepository.SetEmailStatus(email.Id, EmailStatus.Pending);
            
            var client = new SmtpClient();
            client.Connect(settings.Host, settings.Port, settings.ssl);
            client.Authenticate(settings.User, _encryptionLogic.Decrypt(settings.Password));
            
            var rawServerResponse = client.Send(mm);
            client.Disconnect(true);
     
            _emailRepository.SetEmailStatus(email.Id, EmailStatus.Sent);
            _mailServerRepository.UpdateLastUsed(email.ServerId);
            return _emailTransactionRepository.Create(email.Id, rawServerResponse);
        }
        catch (Exception ex)
        {
            _emailRepository.SetEmailStatus(email.Id, EmailStatus.Failed);
            return _emailTransactionRepository.Create(email.Id, ex.Message);
        }
    }
}