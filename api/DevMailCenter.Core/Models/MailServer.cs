namespace DevMailCenter.Models;

public class MailServer
{
    public MailServer(string? name)
    {
        Name = name;
    }

    public Guid Id { get; set; }
    public string? Name { get; set; }
    /// <summary>
    /// Smtp = 0
    /// MicrosoftExchange = 1
    /// </summary>
    public MailServerType Type { get; set; }
    public bool Active { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime LastUsed { get; set; }

    public virtual IEnumerable<MailServerSettings> Settings { get; set; }
}

public enum MailServerType
{
    Smtp,
    MicrosoftExchange
}