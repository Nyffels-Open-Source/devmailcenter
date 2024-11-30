using System.Net.Mail;
using DevMailCenter.External;
using DevMailCenter.External.Models;
using DevMailCenter.Models;
using DevMailCenter.Repository;
using DevMailCenter.Security;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Logic;

public interface IMicrosoftLogic
{
    Guid Send(MicrosoftSettings settings, Email email);
}

public class MicrosoftLogic : IMicrosoftLogic
{
    private readonly ILogger<MicrosoftLogic> _logger;
    private readonly IEmailRepository _emailRepository;
    private readonly IEmailTransactionRepository _emailTransactionRepository;
    private readonly IEncryptionLogic _encryptionLogic;
    private readonly IMicrosoftApi _microsoftApi;
    
    public MicrosoftLogic(ILogger<MicrosoftLogic> logger, IEmailRepository emailRepository, IEmailTransactionRepository emailTransactionRepository, IEncryptionLogic encryptionLogic, IMicrosoftApi microsoftApi)
    {
        _logger = logger;
        _emailRepository = emailRepository;
        _emailTransactionRepository = emailTransactionRepository;
        _encryptionLogic = encryptionLogic;
        _microsoftApi = microsoftApi;
    }

    public Guid Send(MicrosoftSettings settings, Email email)
    {
        var refreshToken = _encryptionLogic.Decrypt(settings.RefreshToken);
        var newTokens = _microsoftApi.GetTokensByRefreshToken(refreshToken);

        var importance = email.Priority switch
        {
          MailPriority.Low => "low",
          MailPriority.Normal => "normal",
          MailPriority.High => "high",
          _ => "normal"
        };

        var toRecipients = new List<MicrosoftApiMailMessageRecipient>();
        var ccRecipients = new List<MicrosoftApiMailMessageRecipient>();
        var bccRecipients = new List<MicrosoftApiMailMessageRecipient>();
        
        foreach (var emailReceiver in email.Receivers)
        {
            var microsoftReceiver = new MicrosoftApiMailMessageRecipient()
            {
                EmailAddress = new MicrosoftApiMailMessageRecipientEmailAddress()
                {
                    Address = emailReceiver.ReceiverEmail
                }
            };

            switch (emailReceiver.Type)
            {
                case EmailReceiverType.CC:
                    ccRecipients.Add(microsoftReceiver);
                    break;
                case EmailReceiverType.BCC:
                    bccRecipients.Add(microsoftReceiver);
                    break;
                default:
                    toRecipients.Add(microsoftReceiver);
                    break;
            }
        }
        
        var mm = new MicrosoftApiMail()
        {
            Message = new MicrosoftApiMailMessage()
            {
                Body = new MicrosoftApiMailMessageBody()
                {
                    Content = email.Message,
                    Type = "html"
                },
                Subject = email.Subject,
                Importance = importance,
                ToRecipients = toRecipients,
                CcRecipients = toRecipients,
                BccRecipients = bccRecipients,
                InterferenceClassification = "focused",
                HasAttachments = false, // TODO Attachments #38
                Attachments = new List<MicrosoftApiMailMessageAttachment>() // TODO Attachments #38
            },
            SaveToSentItems = true
        };

        try
        {
            _emailRepository.SetEmailStatus(email.Id, EmailStatus.Pending);
            _microsoftApi.SendEmail(mm, newTokens);
            _emailRepository.SetEmailStatus(email.Id, EmailStatus.Sent);
            return _emailTransactionRepository.Create(email.Id, "Accepted");
        }
        catch (Exception ex)
        {
            _emailRepository.SetEmailStatus(email.Id, EmailStatus.Failed);
            return _emailTransactionRepository.Create(email.Id, ex.Message);
        }
    }
}