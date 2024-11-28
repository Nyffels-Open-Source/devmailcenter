using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Security;

public interface IEncryptionLogic
{
    bool IsEncryptionEnabled();
    string Encrypt(string value);
    string Decrypt(string value);
    string GenerateEncryptionKey(bool updateEncryptedData = false);
}

public class EncryptionLogic : IEncryptionLogic
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EncryptionLogic> _logger;

    private readonly bool _isEnabled;
    private readonly string _key;
    
    public EncryptionLogic(IConfiguration configuration, ILogger<EncryptionLogic> logger)
    {
        _configuration = configuration;
        _logger = logger;

        _isEnabled = _configuration["Encryption:Enabled"] == "True";
        if (_isEnabled)
        {
            _key = _configuration["Encryption:Key"];   
        }
    }

    public bool IsEncryptionEnabled()
    {
        return _isEnabled;
    }

    public string Encrypt(string value)
    {
        if (_isEnabled)
        {
            // TODO Encrypt
            return "";
        }
        else
        {
            return value;
        }
    }

    public string Decrypt(string value)
    {
        if (_isEnabled)
        {
            // TODO Decrypt
            return "";
        }
        else
        {
            return value;
        }
    }

    public string GenerateEncryptionKey(bool updateEncryptedData = false)
    {
        // TODO Generate a new random Encryption key
        var key = "";

        if (updateEncryptedData)
        {
            // TODO Load mailserversettings with encrypted data parameters. 
            // TODO if encrypted, use key to decrypt.
            // TODO encrypt plain text values to new values with the new key.
        }
        
        return key;
    }
}