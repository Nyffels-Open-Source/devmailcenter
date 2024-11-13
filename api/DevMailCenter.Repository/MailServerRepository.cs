using DevMailCenter.Core;
using DevMailCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace DevMailCenter.Repository;

public interface IMailServerRepository
{
    MailServer Get(Guid id);
    Guid Create(MailServerCreate mailServer);
    int Delete(Guid guid);
    void Update(Guid id, MailServerUpdate mailServer);
}

public class MailServerRepository : IMailServerRepository
{
    private readonly DmcContext _dbContext;

    public MailServerRepository(DmcContext dbContext)
    {
        _dbContext = dbContext;
    }

    public MailServer? Get(Guid id)
    {
        return _dbContext.MailServers.FirstOrDefault(e => e.Id == id);
    }

    public Guid Create(MailServerCreate mailServer)
    {
        var newMailServer = new MailServer(name: mailServer.Name, type: mailServer.Type);
        _dbContext.MailServers.Add(newMailServer);
        _dbContext.SaveChanges();

        return newMailServer.Id;
    }

    public void Update(Guid id, MailServerUpdate mailServer)
    {
        var entry = _dbContext.MailServers.FirstOrDefault(e => e.Id == id);
        if (entry == null) 
        {
            throw new Exception("Mailserver not found");
        }
        
        entry.Name = mailServer.Name;
        entry.Active = mailServer.Active;
        entry.Modified = DateTime.UtcNow;;

        _dbContext.SaveChanges();
    }

    public int Delete(Guid guid)
    {
        return _dbContext.MailServers.Where(e => e.Id == guid).ExecuteDelete();
    }
}