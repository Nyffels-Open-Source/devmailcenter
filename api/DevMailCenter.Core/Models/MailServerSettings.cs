namespace DevMailCenter.Models;

public class MailServerSettings
{
    public MailServerSettings(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public Guid Id { get; set; }
    public Guid ServerId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    public virtual MailServer server { get; set; }
}