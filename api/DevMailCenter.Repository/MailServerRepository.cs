using DevMailCenter.Core;
using DevMailCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace DevMailCenter.Repository;

public interface IMailServerRepository
{
    MailServer Get(Guid id);
    MailServer GetByName(string name);
    Guid Create(MailServerCreate mailServer);
    int Delete(Guid guid);
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
        return _dbContext.MailServers.Where(e => e.Id == id).FirstOrDefault();
    }

    public MailServer GetByName(string name)
    {
        return _dbContext.MailServers.Where(e => e.Name == name).FirstOrDefault();
    }

    public Guid Create(MailServerCreate mailServer)
    {
        var newMailServer = new MailServer(name: mailServer.Name, type: mailServer.Type);
        _dbContext.MailServers.Add(newMailServer);
        _dbContext.SaveChanges();

        return newMailServer.Id;
    }

    public int Delete(Guid guid)
    {
        return _dbContext.MailServers.Where(e => e.Id == guid).ExecuteDelete();
    }
}