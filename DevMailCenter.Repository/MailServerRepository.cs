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
    Guid CreateSmtp(SmtpMailServerCreate mailServer);
    Task<Guid> CreateMicrosoft(MicrosoftMailServerCreate mailServer);
    int Delete(Guid guid);
    void Update(Guid id, MailServerUpdate mailServer);
}

public class MailServerRepository : IMailServerRepository
{
    private readonly DmcContext _dbContext;
    private readonly ILogger<EmailRepository> _logger;
    private readonly IMicrosoftApi _microsoftApi;

    public MailServerRepository(DmcContext dbContext, ILogger<EmailRepository> logger, IMicrosoftApi microsoftApi)
    {
        _dbContext = dbContext;
        _logger = logger;
        _microsoftApi = microsoftApi;
    }

    public MailServer Get(Guid id, bool includeSettings = false)
    {
        var queryable = _dbContext.MailServers.AsQueryable();

        if (includeSettings)
        {
            queryable = queryable.Include(e => e.MailServerSettings.Where(e => e.Secret == false));
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

    public Guid CreateSmtp(SmtpMailServerCreate mailServer)
    {
        var newMailServerId = Guid.NewGuid();
        var newMailServer = new MailServer
        {
            Id = newMailServerId,
            Active = true,
            Created = DateTime.UtcNow,
            Name = mailServer.Name,
            Type = MailServerType.Smtp,
            MailServerSettings = new List<MailServerSettings>()
        };

        newMailServer.MailServerSettings.Add(new MailServerSettings()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Key = "host",
            Value = mailServer.Host,
            ServerId = newMailServerId,
            Secret = false
        });
        newMailServer.MailServerSettings.Add(new MailServerSettings()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Key = "port",
            Value = mailServer.Port.ToString(),
            ServerId = newMailServerId,
            Secret = false
        });
        newMailServer.MailServerSettings.Add(new MailServerSettings()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Key = "ssl",
            Value = mailServer.Ssl.ToString(),
            ServerId = newMailServerId,
            Secret = false
        });
        newMailServer.MailServerSettings.Add(new MailServerSettings()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Key = "email",
            Value = mailServer.Email,
            ServerId = newMailServerId,
            Secret = false
        });
        newMailServer.MailServerSettings.Add(new MailServerSettings()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Key = "user",
            Value = mailServer.User,
            ServerId = newMailServerId,
            Secret = false
        });
        newMailServer.MailServerSettings.Add(new MailServerSettings()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Key = "password",
            Value = mailServer.Password,
            ServerId = newMailServerId,
            Secret = true
        });
        newMailServer.MailServerSettings.Add(new MailServerSettings()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Key = "username",
            Value = mailServer.Username,
            ServerId = newMailServerId,
            Secret = false
        });

        _dbContext.MailServers.Add(newMailServer);
        _dbContext.SaveChanges();

        return newMailServer.Id;
    }

    public async Task<Guid> CreateMicrosoft(MicrosoftMailServerCreate mailServer)
    {
        var MicrosoftTokens = _microsoftApi.GetTokensByOnBehalfAccessToken(mailServer.Code);
        
        var newMailServerId = Guid.NewGuid();
        var newMailServer = new MailServer
        {
            Id = newMailServerId,
            Active = true,
            Created = DateTime.UtcNow,
            Name = "Microsoft Exchange 365",
            Type = MailServerType.MicrosoftExchange,
            MailServerSettings = new List<MailServerSettings>()
        };
        
        newMailServer.MailServerSettings.Add(new MailServerSettings()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Key = "refreshToken",
            Value = MicrosoftTokens.RefreshToken,
            ServerId = newMailServerId,
            Secret = true
        });
        
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