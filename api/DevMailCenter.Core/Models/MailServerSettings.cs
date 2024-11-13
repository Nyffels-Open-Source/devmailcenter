namespace DevMailCenter.Models;

public class MailServerSettings
{
    public MailServerSettings(string key, string value, Guid serverId)
    {
        Key = key;
        Value = value;
        ServerId = serverId;
        Created = DateTime.UtcNow;
    }

    public Guid Id { get; set; }
    public Guid ServerId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}

public class MailServerSettingsMutation
{
    public MailServerSettingsMutation(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; set; }
    public string Value { get; set; }
}