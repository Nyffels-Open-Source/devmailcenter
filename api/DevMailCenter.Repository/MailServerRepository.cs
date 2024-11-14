using DevMailCenter.Core;
using DevMailCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace DevMailCenter.Repository;

public interface IMailServerRepository
{
    MailServer Get(Guid id, bool includeSettings = true);
    List<MailServer> List(bool includeSettings = false);
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

    public MailServer Get(Guid id, bool includeSettings = false)
    {
        var queryable = _dbContext.MailServers.AsQueryable();

        if (includeSettings)
        {
            queryable = queryable.Include(e => e.MailServerSettings);
        }
        
        return queryable.FirstOrDefault(e => e.Id == id);
    }

    public List<MailServer> List(bool includeSettings = false)
    {
        var queryable = _dbContext.MailServers.AsQueryable();
        
        if (includeSettings)
        {
            queryable = queryable.Include(e => e.MailServerSettings);
        }
        
        return queryable.ToList();
    }

    public Guid Create(MailServerCreate mailServer)
    {
        var settingsMissing = new List<string>();
        var keys = mailServer.Settings.Select(e => e.Key);

        switch (mailServer.Type)
        {
            case MailServerType.Smtp:
            {
                var smtpValues = new List<string>() { "host", "port", "ssl", "email", "user", "password" };

                if (!keys.Contains("host"))
                {
                    settingsMissing.Add("host");
                }

                if (!keys.Contains("port"))
                {
                    settingsMissing.Add("port");
                }

                if (!keys.Contains("ssl"))
                {
                    settingsMissing.Add("ssl");
                }

                if (!keys.Contains("email"))
                {
                    settingsMissing.Add("email");
                }

                if (!keys.Contains("user"))
                {
                    settingsMissing.Add("user");
                }

                if (!keys.Contains("password"))
                {
                    settingsMissing.Add("password");
                }

                mailServer.Settings = mailServer.Settings.Where(e => smtpValues.Contains(e.Key)).ToList();

                break;
            }
            case MailServerType.MicrosoftExchange:
            {
                // TODO Accept authorization token or refresh token?
                break;
            }
        }

        if (settingsMissing.Count > 0)
        {
            throw new Exception(String.Format("Missing settings: {0}", String.Join(", ", settingsMissing)));
        }
        
        var newMailServerId = Guid.NewGuid();
        var newMailServer = new MailServer
        {
            Id = newMailServerId,
            Active = true,
            Created = DateTime.UtcNow,
            Name = mailServer.Name,
            Type = mailServer.Type,
            MailServerSettings = mailServer.Settings.Select(setting => new MailServerSettings
            {
                Created = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                ServerId = newMailServerId,
                Key = setting.Key,
                Value = setting.Value,
            }).ToList()
        };
        _dbContext.MailServers.Add(newMailServer);
        _dbContext.SaveChanges();

        return newMailServer.Id;
    }

    public void Update(Guid id, MailServerUpdate mailServer)
    {
        var entry = _dbContext.MailServers.Include(e => e.MailServerSettings).FirstOrDefault(e => e.Id == id);
        if (entry == null)
        {
            throw new Exception("Mailserver not found");
        }

        entry.Name = mailServer.Name;
        entry.Active = mailServer.Active;
        entry.Modified = DateTime.UtcNow;

        foreach (var setting in mailServer.Settings)
        {
            entry.MailServerSettings.First(e => e.Key == setting.Key).Value = setting.Value;
        }

        _dbContext.SaveChanges();
    }

    public int Delete(Guid guid)
    {
        return _dbContext.MailServers.Where(e => e.Id == guid).ExecuteDelete();
    }
}