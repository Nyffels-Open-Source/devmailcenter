using System.ComponentModel.DataAnnotations.Schema;

namespace DevMailCenter.Models;

[Table("MailServer")]
public class MailServer
{
    [Column("ServerId")]
    public Guid Id { get; set; }
    
    [Column("ServerName")]
    public string Name { get; set; }
    
    [Column("ServerType")]
    public MailServerType Type { get; set; }

    [Column("ServerActive")]
    public bool Active { get; set; }
    
    [Column("ServerCreated")]
    public DateTime Created { get; set; }
    
    [Column("ServerModified")]
    public DateTime Modified { get; set; }
    
    [Column("ServerLastUsed")]
    public DateTime LastUsed { get; set; }
}

public enum MailServerType
{
    Smtp = 0,
    MicrosoftExchange = 1
}