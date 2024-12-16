using DevMailCenter.Models;

namespace DevMailCenter.Repository;

public interface IEmailAttachmentRepository
{
    List<EmailAttachment> AddToStorage(List<EmailAttachmentCreate> emailAttachments, Guid emailId);
}

public class EmailAttachmentRepository : IEmailAttachmentRepository
{
    public EmailAttachmentRepository()
    {
        
    }

    public List<EmailAttachment> AddToStorage(List<EmailAttachmentCreate> emailAttachments, Guid emailId)
    {
        var attachments = new List<EmailAttachment>();
        
        foreach (EmailAttachmentCreate emailAttachment in emailAttachments)
        {
            var newAttachmentId = Guid.NewGuid();
            
            // TODO Save the base64 to the storage
            
            attachments.Add(new EmailAttachment()
            {
                Id = newAttachmentId,
                EmailId = emailId,
                Mime = emailAttachment.Mime,
                Name = emailAttachment.Name,
            });
        }
        
        return attachments;
    }

    public void DeleteFromStorage(List<Guid> attachmentGuids)
    {
        // TODO Delete the base64 from the storage based on the guids. 
    }
}