using System.ComponentModel.DataAnnotations.Schema;

namespace DevMailCenter.Models;

[Table("MailServerSettings")]
public class MailServerSettings
{
    [Column("ServerSettingsGuid")]
    public Guid Id { get; set; }
    
    [Column("ServerSettingsKey")]
    public string Key { get; set; }
    
    [Column("ServerSettingsValue")]
    public string Value { get; set; }
    
    [Column("MailServerSettingsCreated")]
    public DateTime Created { get; set; }
    
    [Column("MailServerSettingsModified")]
    public DateTime Modified { get; set; }
}