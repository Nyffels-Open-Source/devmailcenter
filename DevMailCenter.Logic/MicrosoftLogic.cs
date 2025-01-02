using System.Net.Mail;
using System.Text;
using DevMailCenter.External;
using DevMailCenter.External.Models;
using DevMailCenter.Models;
using DevMailCenter.Repository;
using DevMailCenter.Security;
using MailKit;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Logic;

public interface IMicrosoftLogic
{
    Task<Guid> Send(MicrosoftSettings settings, Email email);
}

public class MicrosoftLogic : IMicrosoftLogic
{
    private readonly ILogger<MicrosoftLogic> _logger;
    private readonly IEmailRepository _emailRepository;
    private readonly IEmailTransactionRepository _emailTransactionRepository;
    private readonly IEncryptionLogic _encryptionLogic;
    private readonly IMicrosoftApi _microsoftApi;
    private readonly IMailServerRepository _mailServerRepository;
    private readonly IEmailAttachmentRepository _emailAttachmentRepository;

    public MicrosoftLogic(ILogger<MicrosoftLogic> logger, IEmailRepository emailRepository, IEmailTransactionRepository emailTransactionRepository, IEncryptionLogic encryptionLogic, IMicrosoftApi microsoftApi, IMailServerRepository mailServerRepository,
        IEmailAttachmentRepository emailAttachmentRepository)
    {
        _logger = logger;
        _emailRepository = emailRepository;
        _emailTransactionRepository = emailTransactionRepository;
        _encryptionLogic = encryptionLogic;
        _microsoftApi = microsoftApi;
        _mailServerRepository = mailServerRepository;
        _emailAttachmentRepository = emailAttachmentRepository;
    }

    public async Task<Guid> Send(MicrosoftSettings settings, Email email)
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
                CcRecipients = ccRecipients,
                BccRecipients = bccRecipients,
                InterferenceClassification = "focused",
                HasAttachments = email.Attachments.Count > 0,
                Attachments = email.Attachments.Select(att =>
                {
                    var content = _emailAttachmentRepository.GetAttachmentBytes(att.Id);
                    if (content == null)
                    {
                        throw new Exception($"Attachment {att.Id} no longer exists in storage.");
                    }

                    return new MicrosoftApiMailMessageAttachment()
                    {
                        Type = "#microsoft.graph.fileAttachment",
                        Name = att.Name,
                        ContentType = att.Mime,
                        ContentBytes = content!
                    };
                }).ToList()
            },
            SaveToSentItems = true
        };

        try
        {
            _emailRepository.SetEmailStatus(email.Id, EmailStatus.Pending);
            await _microsoftApi.SendEmail(mm, newTokens);
            _emailRepository.SetEmailStatus(email.Id, EmailStatus.Sent);
            _mailServerRepository.UpdateLastUsed(email.ServerId);
            return _emailTransactionRepository.Create(email.Id, "Ok");
        }
        catch (Exception ex)
        {
            _emailRepository.SetEmailStatus(email.Id, EmailStatus.Failed);
            return _emailTransactionRepository.Create(email.Id, ex.Message);
        }
    }

    private Stream ConvertAttachmentStreamToBase64(Stream stream)
    {
        Byte[] inArray = new Byte[(int)stream.Length];
        Char[] outArray = new Char[(int)(stream.Length * 1.34)];
        stream.Read(inArray, 0, (int)stream.Length);
        Convert.ToBase64CharArray(inArray, 0, inArray.Length, outArray, 0);
        return new MemoryStream(Encoding.UTF8.GetBytes(outArray));
    }
}