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

public class MailServerCreate(string name, MailServerType type)
{
    public string Name { get; set; } = name;
    public MailServerType Type { get; set; } = type;
    public List<MailServerSettingsMutation> Settings { get; set; } = new();
}

public class SmtpMailServerCreate(
    string name,
    string host,
    int port,
    bool ssl,
    string email,
    string user,
    string password,
    string username)
{
    public string Name { get; set; } = name;
    public string Host { get; set; } = host;
    public int Port { get; set; } = port;
    public bool Ssl { get; set; } = ssl;
    public string Email { get; set; } = email;
    public string User { get; set; } = user;
    public string Password { get; set; } = password;
    public string Username { get; set; } = username;
}

public class SmtpMailServerUpdate(
    string name,
    bool active,
    string host,
    int port,
    bool ssl,
    string email,
    string user,
    string password,
    string username)
{
    public string Name { get; set; } = name;
    public bool Active { get; set; } = active;
    public string Host { get; set; } = host;
    public int Port { get; set; } = port;
    public bool Ssl { get; set; } = ssl;
    public string Email { get; set; } = email;
    public string User { get; set; } = user;
    public string Password { get; set; } = password;
    public string Username { get; set; } = username;
}

public class MicrosoftMailServerCreate(string code)
{
    public string Code { get; set; } = code;
}

public class MicrosoftMailServerUpdate(string name, bool active)
{
    public string Name { get; set; } = name;
    public bool Active { get; set; } = active;
}

public enum MailServerType
{
    Smtp,
    MicrosoftExchange,
    Google
}