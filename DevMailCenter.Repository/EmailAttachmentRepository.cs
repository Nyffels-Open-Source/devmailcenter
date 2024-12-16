using DevMailCenter.Models;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Repository;

public interface IEmailAttachmentRepository
{
    List<EmailAttachment> AddToStorage(List<EmailAttachmentCreate> emailAttachments, Guid emailId);
    void DeleteFromStorage(List<Guid> attachmentGuids);
    string GetAttachmentContent(Guid guid);
    Stream GetAttachmentStream(Guid guid);
}

public class EmailAttachmentRepository : IEmailAttachmentRepository
{
    private string _rootPath { get; init; }
    private readonly ILogger<EmailAttachmentRepository> _logger;
    
    public EmailAttachmentRepository(ILogger<EmailAttachmentRepository> logger)
    {
        var attachmentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Attachments");
        if (!Directory.Exists(attachmentPath))
        {
            Directory.CreateDirectory(attachmentPath);   
        }
        
        _rootPath = attachmentPath;
        _logger = logger;
    }

    public List<EmailAttachment> AddToStorage(List<EmailAttachmentCreate> emailAttachments, Guid emailId)
    {
        var attachments = new List<EmailAttachment>();
        
        foreach (EmailAttachmentCreate emailAttachment in emailAttachments)
        {
            var newAttachmentId = Guid.NewGuid();
            File.WriteAllBytes(Path.Combine(_rootPath, newAttachmentId.ToString()), Convert.FromBase64String(emailAttachment.Base64));
            attachments.Add(new EmailAttachment()
            {
                Id = newAttachmentId,
                EmailId = emailId,
                Mime = emailAttachment.Mime,
                Name = emailAttachment.Name,
            });
            _logger.LogInformation($"Added attachment '{newAttachmentId}' with name {emailAttachment.Name}");
        }
        
        return attachments;
    }

    public void DeleteFromStorage(List<Guid> attachmentGuids)
    {
        foreach (var guid in attachmentGuids)
        {
            File.Delete(Path.Combine(_rootPath, guid.ToString()));   
            _logger.LogInformation($"Removed attachment '{guid}'.");
        }
    }

    public string GetAttachmentContent(Guid guid)
    {
        // TODO Read file and return base64. 
        return "";
    }

    public Stream GetAttachmentStream(Guid guid)
    {
        return File.OpenRead(Path.Combine(_rootPath, guid.ToString()));
    }
}