namespace DevMailCenter.Models;

public class MailServerSettings
{
    public Guid Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}