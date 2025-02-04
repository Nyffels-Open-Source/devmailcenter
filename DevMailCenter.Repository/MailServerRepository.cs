﻿using DevMailCenter.Core;
using DevMailCenter.External;
using DevMailCenter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DevMailCenter.Security;

namespace DevMailCenter.Repository;

public interface IMailServerRepository
{
    MailServer Get(Guid id, bool includeSettings = false, bool includeSecrets = false);
    List<MailServer> List(bool includeSettings = false, bool includeSecrets = false);
    Guid CreateSmtp(SmtpMailServerCreate mailServer);
    Task<Guid> CreateMicrosoft(MicrosoftMailServerCreate mailServer);
    int Delete(Guid guid);
    void UpdateSmtp(Guid id, SmtpMailServerUpdate mailServer);
    void UpdateMicrosoft(Guid id, MicrosoftMailServerUpdate mailServer);
    void UpdateLastUsed(Guid guid);

}

public class MailServerRepository : IMailServerRepository
{
    private readonly DmcContext _dbContext;
    private readonly ILogger<EmailRepository> _logger;
    private readonly IMicrosoftApi _microsoftApi;
    private readonly IEncryptionLogic _encryptionLogic;

    public MailServerRepository(DmcContext dbContext, ILogger<EmailRepository> logger, IMicrosoftApi microsoftApi, IEncryptionLogic encryptionLogic)
    {
        _dbContext = dbContext;
        _logger = logger;
        _microsoftApi = microsoftApi;
        _encryptionLogic = encryptionLogic;
    }

    public MailServer Get(Guid id, bool includeSettings = false, bool includeSecrets = false)
    {
        var queryable = _dbContext.MailServers.AsQueryable();

        if (includeSettings && includeSecrets)
        {
            queryable = queryable.Include(e => e.MailServerSettings);
        }
        else if (includeSettings && !includeSecrets)
        {
            queryable = queryable.Include(e => e.MailServerSettings.Where(e => e.Secret == false));
        }

        return queryable.FirstOrDefault(e => e.Id == id);
    }

    public List<MailServer> List(bool includeSettings = false, bool includeSecrets = false)
    {
        var queryable = _dbContext.MailServers.AsQueryable();

        if (includeSettings && includeSecrets)
        {
            queryable = queryable.Include(e => e.MailServerSettings);
        }
        else if (includeSettings && !includeSecrets)
        {
            queryable = queryable.Include(e => e.MailServerSettings.Where(e => e.Secret == false));
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
            Value = _encryptionLogic.Encrypt(mailServer.Password),
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
        var MicrosoftTokens = await _microsoftApi.GetTokensByOnBehalfAccessToken(mailServer.Code);
        
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
            Value = _encryptionLogic.Encrypt(MicrosoftTokens.RefreshToken),
            ServerId = newMailServerId,
            Secret = true
        });
        
        _dbContext.MailServers.Add(newMailServer);
        await _dbContext.SaveChangesAsync();
        
        return newMailServer.Id;
    }

    public void UpdateSmtp(Guid id, SmtpMailServerUpdate mailServer)
    {
        var entry = _dbContext.MailServers.Include(e => e.MailServerSettings).FirstOrDefault(e => e.Id == id);
        if (entry == null)
        {
            throw new Exception("Mailserver not found");
        }
        
        if (entry.Type != MailServerType.Smtp)
        {
            throw new Exception("Mailserver type isn't Smtp");
        }
        
        entry.Name = mailServer.Name;
        entry.Active = mailServer.Active;
        entry.Modified = DateTime.UtcNow;
        
        entry.MailServerSettings.Single(e => e.Key == "host").Value = mailServer.Host;
        entry.MailServerSettings.Single(e => e.Key == "host").Modified = DateTime.UtcNow;
        
        entry.MailServerSettings.Single(e => e.Key == "port").Value = mailServer.Port.ToString();
        entry.MailServerSettings.Single(e => e.Key == "port").Modified = DateTime.UtcNow;
        
        entry.MailServerSettings.Single(e => e.Key == "ssl").Value = mailServer.Ssl.ToString();
        entry.MailServerSettings.Single(e => e.Key == "ssl").Modified = DateTime.UtcNow;
        
        entry.MailServerSettings.Single(e => e.Key == "email").Value = mailServer.Email;
        entry.MailServerSettings.Single(e => e.Key == "email").Modified = DateTime.UtcNow;
        
        entry.MailServerSettings.Single(e => e.Key == "user").Value = mailServer.User;
        entry.MailServerSettings.Single(e => e.Key == "user").Modified = DateTime.UtcNow;
        
        entry.MailServerSettings.Single(e => e.Key == "password").Value = _encryptionLogic.Encrypt(mailServer.Password);
        entry.MailServerSettings.Single(e => e.Key == "password").Modified = DateTime.UtcNow;
        
        entry.MailServerSettings.Single(e => e.Key == "username").Value = mailServer.Username;
        entry.MailServerSettings.Single(e => e.Key == "username").Modified = DateTime.UtcNow;
        
        _dbContext.SaveChanges();
    }

    public void UpdateMicrosoft(Guid id, MicrosoftMailServerUpdate mailServer)
    {
        var entry = _dbContext.MailServers.FirstOrDefault(e => e.Id == id);
        if (entry == null)
        {
            throw new Exception("Mailserver not found");
        }
        
        if (entry.Type != MailServerType.MicrosoftExchange)
        {
            throw new Exception("Mailserver type isn't Microsoft Exchange");
        }
        
        entry.Name = mailServer.Name;
        entry.Active = mailServer.Active;
        entry.Modified = DateTime.UtcNow;
        
        _dbContext.SaveChanges();
    }

    public int Delete(Guid guid)
    {
        return _dbContext.MailServers.Where(e => e.Id == guid).ExecuteDelete();
    }

    public void UpdateLastUsed(Guid guid)
    {
        var server = _dbContext.MailServers.Where(e => e.Id == guid).FirstOrDefault();
        if (server == null)
        {
            throw new Exception("Request for lastused update failed because server was not found.");
        }
        
        server.LastUsed = DateTime.UtcNow;
        _dbContext.SaveChanges();
    }
}