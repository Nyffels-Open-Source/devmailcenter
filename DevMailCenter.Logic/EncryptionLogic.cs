using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Logic;

public interface IEncryptionLogic
{
    
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
}