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

public class EmailRepository : IEmailRepository
{
    private readonly DmcContext _dbContext;

    public EmailRepository(DmcContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Email Get(Guid id, bool includeReceivers = false)
    {
        var queryable = _dbContext.Emails.AsQueryable();

        if (includeReceivers)
        {
            queryable = queryable.Include(e => e.Receivers);
        }

        return queryable.FirstOrDefault(e => e.Id == id);
    }

    public List<Email> List(bool includeReceivers = false)
    {
        var queryable = _dbContext.Emails.AsQueryable();

        if (includeReceivers)
        {
            queryable = queryable.Include(e => e.Receivers);
        }

        return queryable.ToList();
    }

    public Guid Create(EmailCreate email, Guid serverId)
    {
        var newEmailId = new Guid();
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
            Modified = null, 
            Completed = null, 
            SendRequested = null,
        };
        _dbContext.Add(newEmail);
        _dbContext.SaveChanges();

        return newEmail.Id;
    }

    public void Update(Guid id, EmailUpdate email)
    {
        var entry = _dbContext.Emails.Include(e => e.Receivers).FirstOrDefault(e => e.Id == id);
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

        if (email.DeletedReceivers != null && email.DeletedReceivers.Count > 0)
        {
            entry.Receivers = entry.Receivers.Where(receiver => !email.DeletedReceivers.Contains(receiver.Id)).ToList();
        }

        if (email.UpdatedReceivers != null && email.UpdatedReceivers.Count > 0)
        {
            foreach (var receiver in email.UpdatedReceivers)
            {
                entry.Receivers.First(e => e.Id == receiver.Id).ReceiverEmail = receiver.ReceiverEmail;
                entry.Receivers.First(e => e.Id == receiver.Id).Type = receiver.Type;
            }
        }

        if (email.CreatedReceivers != null && email.CreatedReceivers.Count > 0)
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

        _dbContext.SaveChanges();
    }

    public int Delete(Guid guid)
    {
        return _dbContext.Emails.Where(e => e.Id == guid).ExecuteDelete();
    }

    public void SetEmailStatus(Guid id, EmailStatus status)
    {
        var entry = _dbContext.Emails.FirstOrDefault(e => e.Id == id);
        entry.Status = status;
        _dbContext.SaveChanges();
    }
}