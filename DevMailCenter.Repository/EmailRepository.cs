using DevMailCenter.Core;
using DevMailCenter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Repository;

public interface IEmailRepository
{
    Email Get(Guid id, bool includeReceivers = false);
    List<Email> List(bool includeReceivers = false);
    Guid Create(EmailCreate email, Guid serverId);
    void Update(Guid id, EmailUpdate email);
    int Delete(Guid guid);
    void SetEmailStatus(Guid id, EmailStatus status);
}

public class EmailRepository(DmcContext dbContext, IEmailAttachmentRepository emailAttachmentRepository) : IEmailRepository
{
    public Email Get(Guid id, bool includeReceivers = false)
    {
        var queryable = dbContext.Emails.AsQueryable();

        if (includeReceivers)
        {
            queryable = queryable.Include(e => e.Receivers);
        }

        var email = queryable.FirstOrDefault(e => e.Id == id);
        var server = dbContext.MailServers.FirstOrDefault(e => e.Id == email.ServerId);
        if (server != null)
        {
            email.ServerName = server.Name;
        }

        return email;
    }

    public List<Email> List(bool includeReceivers = false)
    {
        var queryable = dbContext.Emails.AsQueryable();

        if (includeReceivers)
        {
            queryable = queryable.Include(e => e.Receivers);
        }

        var emails = queryable.ToList();
        
        var servers = dbContext.MailServers.Where(s => emails.Select(e => e.ServerId).Contains(s.Id)).ToList();
        
        emails.ForEach(e =>
        {
            e.ServerName = servers.FirstOrDefault(s => s.Id == e.ServerId)?.Name;
        });
        return emails;
    }

    public Guid Create(EmailCreate email, Guid serverId)
    {
        var newEmailId = new Guid();
        var attachments = emailAttachmentRepository.AddToStorage(email.Attachments, newEmailId);
        var newEmail = new Email()
        {
            Created = DateTime.Now,
            Id = newEmailId,
            Message = email.Message,
            Status = EmailStatus.Concept,
            Subject = email.Subject,
            Priority = email.Priority,
            ServerId = serverId,
            Receivers = email.Receivers.Select(receiver => new EmailReceiver()
            {
                Type = receiver.Type,
                Id = Guid.NewGuid(),
                ReceiverName = receiver.ReceiverName,
                ReceiverEmail = receiver.ReceiverEmail,
                EmailId = newEmailId
            }).ToList(),
            Attachments = attachments,
            Modified = null, 
            Completed = null, 
            SendRequested = null,
        };
        dbContext.Add(newEmail);
        dbContext.SaveChanges();

        return newEmail.Id;
    }

    public void Update(Guid id, EmailUpdate email)
    {
        var entry = dbContext.Emails.Include(e => e.Receivers).Include(email => email.Attachments).FirstOrDefault(e => e.Id == id);
        if (entry == null)
        {
            throw new Exception("Email not found");
        }

        if (entry.Status != EmailStatus.Concept)
        {
            throw new Exception("Email isn't in an editable state");
        }

        entry.Subject = email.Subject;
        entry.Message = email.Message;
        entry.Modified = DateTime.UtcNow;

        if (email.DeletedReceivers.Count > 0)
        {
            entry.Receivers = entry.Receivers.Where(receiver => !email.DeletedReceivers.Contains(receiver.Id)).ToList();
        }

        if (email.UpdatedReceivers.Count > 0)
        {
            foreach (var receiver in email.UpdatedReceivers)
            {
                entry.Receivers.First(e => e.Id == receiver.Id).ReceiverEmail = receiver.ReceiverEmail;
                entry.Receivers.First(e => e.Id == receiver.Id).Type = receiver.Type;
            }
        }

        if (email.CreatedReceivers.Count > 0)
        {
            foreach (var receiver in email.CreatedReceivers)
            {
                entry.Receivers.Add(new EmailReceiver
                {
                    Type = receiver.Type,
                    ReceiverName = receiver.ReceiverName,
                    ReceiverEmail = receiver.ReceiverEmail,
                    EmailId = id,
                    Id = Guid.NewGuid(),
                });   
            }
        }

        if (email.DeletedAttachments.Count > 0)
        {
            emailAttachmentRepository.DeleteFromStorage(email.DeletedAttachments);
            entry.Attachments = entry.Attachments.Where(attachment => !email.DeletedAttachments.Contains(attachment.Id)).ToList();
        }

        if (email.AddedAttachments.Count > 0)
        {
            entry.Attachments = emailAttachmentRepository.AddToStorage(email.AddedAttachments, entry.Id);
        }

        dbContext.SaveChanges();
    }

    public int Delete(Guid guid)
    {
        // TODO Retrieve attachment GUIDS. 
        
        return dbContext.Emails.Where(e => e.Id == guid).ExecuteDelete();
    }

    public void SetEmailStatus(Guid id, EmailStatus status)
    {
        var entry = dbContext.Emails.FirstOrDefault(e => e.Id == id);
        entry.Status = status;
        dbContext.SaveChanges();
    }
}