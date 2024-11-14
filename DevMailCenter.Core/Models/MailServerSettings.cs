namespace DevMailCenter.Models;

public class MailServerSettings
{
    public required Guid Id { get; set; }
    public required Guid ServerId { get; set; }
    public required string Key { get; set; }
    public required string Value { get; set; }
    public required DateTime Created { get; set; }
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