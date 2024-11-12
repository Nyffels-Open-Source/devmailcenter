﻿namespace DevMailCenter.Models;

public class MailServer
{
    public MailServer(string name, MailServerType type)
    {
        Name = name;
        Type = type;

        Id = Guid.NewGuid();
        Active = true;
        Created = DateTime.UtcNow;
        Modified = null;
        LastUsed = null;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public MailServerType Type { get; set; }
    public bool Active { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
    public DateTime? LastUsed { get; set; }

    public virtual IEnumerable<MailServerSettings> Settings { get; set; }
}

public class MailServerCreate
{
    public MailServerCreate(string name, MailServerType type)
    {
        Type = type;
        Name = name;
    }

    public string Name { get; set; }
    public MailServerType Type { get; set; }
}

public enum MailServerType
{
    Smtp,
    MicrosoftExchange
}