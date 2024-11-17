using DevMailCenter.Core;
using DevMailCenter.External;
using DevMailCenter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Repository;

public interface IMailServerRepository
{
    MailServer Get(Guid id, bool includeSettings = true);
    List<MailServer> List(bool includeSettings = false);
    Task<Guid> Create(MailServerCreate mailServer);
    int Delete(Guid guid);
    void Update(Guid id, MailServerUpdate mailServer);
}

public class MailServerRepository : IMailServerRepository
{
    private readonly DmcContext _dbContext;
    private readonly ILogger<EmailRepository> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public MailServerRepository(DmcContext dbContext, ILogger<EmailRepository> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _dbContext = dbContext;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
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

    public async Task<Guid> Create(MailServerCreate mailServer)
    {
        var settingsMissing = new List<string>();
        var keys = mailServer.Settings.Select(e => e.Key);

        switch (mailServer.Type)
        {
            case MailServerType.Smtp:
            {
                var smtpValues = new List<string>() { "host", "port", "ssl", "email", "user", "password", "name" };

                foreach (var value in smtpValues)
                {
                    if (!keys.Contains(value))
                    {
                        settingsMissing.Add(value);
                    }
                }

                mailServer.Settings = mailServer.Settings.Where(e => smtpValues.Contains(e.Key)).ToList();
                
                if (settingsMissing.Count > 0)
                {
                    throw new Exception(String.Format("Missing settings: {0}", String.Join(", ", settingsMissing)));
                }

                break;
            }
            case MailServerType.MicrosoftExchange:
            {
                if (keys.FirstOrDefault(e => e == "authorizationcode") == null)
                {
                    throw new Exception("Missing settings: authorizationcode");
                }

                var refreshToken = await _serviceScopeFactory.CreateScope().ServiceProvider
                    .GetRequiredService<IMicrosoftApi>()
                    .GetRefreshTokenByAuthorizationCode(
                        mailServer.Settings.First(e => e.Key == "authorizationcode").Value);

                mailServer.Settings = new List<MailServerSettingsMutation>()
                {
                    new MailServerSettingsMutation("refreshtoken", refreshToken)
                };
                
                break;
            }
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