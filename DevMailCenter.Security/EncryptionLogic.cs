using System.Security.Cryptography;
using DevMailCenter.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DevMailCenter.Security;

public interface IEncryptionLogic
{
    bool IsEncryptionEnabled();
    string Encrypt(string value);
    string Decrypt(string value);
    string GenerateEncryptionKey(bool updateSensitiveData = false);
}

public class EncryptionLogic : IEncryptionLogic
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EncryptionLogic> _logger;
    private readonly DmcContext _dbContext;

    private bool _isEnabled;
    private string  _key;

    public EncryptionLogic(IConfiguration configuration, ILogger<EncryptionLogic> logger, DmcContext dbContext)
    {
        _configuration = configuration;
        _logger = logger;
        _dbContext = dbContext;

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
            return EncryptString(value, _key);
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
            return DecryptString(value, _key);
        }
        else
        {
            return value;
        }
    }

    public string GenerateEncryptionKey(bool updateSensitiveData = true)
    {
        Aes aes = Aes.Create();
        aes.GenerateKey();
        var key = Convert.ToBase64String(aes.Key);

        if (updateSensitiveData)
        {
            var serverSettings = _dbContext.MailServerSettings.Where(e => e.Secret == true).ToList();
            foreach (var setting in serverSettings)
            {
                if (_isEnabled)
                {
                    setting.Value = DecryptString(setting.Value, _key);
                }

                setting.Value = EncryptString(setting.Value, key);
            }
            _dbContext.SaveChanges();
            
            _isEnabled = true;
            _key = key;
        }
        
        return key;
    }

    private string EncryptString(string plaintext, string key)
    {
        byte[] plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plaintext);

        Rfc2898DeriveBytes passwordBytes = new Rfc2898DeriveBytes(key, 20);

        Aes encryptor = Aes.Create();
        encryptor.Key = passwordBytes.GetBytes(32);
        encryptor.IV = passwordBytes.GetBytes(16);
        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(plaintextBytes, 0, plaintextBytes.Length);
            }

            return Convert.ToBase64String(ms.ToArray());
        }
    }

    private string DecryptString(string encrypted, string key)
    {
        byte[] encryptedBytes = Convert.FromBase64String(encrypted);

        Rfc2898DeriveBytes passwordBytes = new Rfc2898DeriveBytes(key, 20);

        Aes encryptor = Aes.Create();
        encryptor.Key = passwordBytes.GetBytes(32);
        encryptor.IV = passwordBytes.GetBytes(16);
        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(encryptedBytes, 0, encryptedBytes.Length);
            }

            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}