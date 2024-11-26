namespace DevMailCenter.Models;

public class MicrosoftTokens
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
}