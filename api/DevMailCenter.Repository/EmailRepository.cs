using DevMailCenter.Core;
using DevMailCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace DevMailCenter.Repository;

public interface IEmailRepository
{
    Email Get(Guid id, bool includeReceivers = false);
    List<Email> List(bool includeReceivers = false);
    Guid Create(EmailCreate email);
    void Update(Guid id, EmailUpdate email);
    int Delete(Guid guid);
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

    public Guid Create(EmailCreate email)
    {
        // TODO
        // var newMailServer = new MailServer(name: mailServer.Name, type: mailServer.Type);
        // _dbContext.MailServers.Add(newMailServer);
        // _dbContext.MailServerSettings.AddRange(mailServer.Settings.Select(e => new MailServerSettings(e.Key, e.Value, newMailServer.Id)));
        //
        // _dbContext.SaveChanges();
        //
        // return newMailServer.Id;
        return new Guid();
    }

    public void Update(Guid id, EmailUpdate email)
    {
        // var entry = _dbContext.MailServers.FirstOrDefault(e => e.Id == id);
        // if (entry == null)
        // {
        //     throw new Exception("Mailserver not found");
        // }
        //
        // entry.Name = mailServer.Name;
        // entry.Active = mailServer.Active;
        // entry.Modified = DateTime.UtcNow;
        // ;
        //
        // _dbContext.SaveChanges();
    }

    public int Delete(Guid guid)
    {
        return _dbContext.Emails.Where(e => e.Id == guid).ExecuteDelete();
    }
}