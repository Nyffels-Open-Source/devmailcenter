namespace DevMailCenter.Models;

public class MailServer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public MailServerType Type { get; set; }
    public bool Active { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime LastUsed { get; set; }
}

public enum MailServerType
{
    Smtp = 0,
    MicrosoftExchange = 1
}