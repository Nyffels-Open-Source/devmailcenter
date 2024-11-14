namespace DevMailCenter.Models;

public class MailServer
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required MailServerType Type { get; set; }
    public required bool Active { get; set; }
    public required DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
    public DateTime? LastUsed { get; set; }

    public required ICollection<MailServerSettings> MailServerSettings { get; set; }
    public ICollection<Email>? Emails { get; set; }
}

public class MailServerCreate
{
    public MailServerCreate(string name, MailServerType type)
    {
        Type = type;
        Name = name;
        Settings = new List<MailServerSettingsMutation>();
    }

    public string Name { get; set; }
    public MailServerType Type { get; set; }
    public List<MailServerSettingsMutation> Settings { get; set; }
}

public class MailServerUpdate
{
    public MailServerUpdate(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public bool Active { get; set; }
    public List<MailServerSettingsMutation> Settings { get; set; }
}

public enum MailServerType
{
    Smtp,
    MicrosoftExchange
}