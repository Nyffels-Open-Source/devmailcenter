using System.Security.Cryptography;
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

    public string GenerateEncryptionKey(bool updateSensitiveData = false)
    {
        Aes aes = Aes.Create();  
        aes.GenerateKey();
        var key = Convert.ToBase64String(aes.Key);

        if (updateSensitiveData)
        {
            // TODO Load mailserversettings with encrypted data parameters. 
            // TODO if encrypted, use key to decrypt.
            // TODO encrypt plain text values to new values with the new key.
        }
        
        return key;
    }
    
    private string EncryptString(string plaintext, string key)
    {
        // Convert the plaintext string to a byte array
        byte[] plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plaintext);
 
        // Derive a new password using the PBKDF2 algorithm and a random salt
        Rfc2898DeriveBytes passwordBytes = new Rfc2898DeriveBytes(key, 20);
 
        // Use the password to encrypt the plaintext
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
        // Convert the encrypted string to a byte array
        byte[] encryptedBytes = Convert.FromBase64String(encrypted);
 
        // Derive the password using the PBKDF2 algorithm
        Rfc2898DeriveBytes passwordBytes = new Rfc2898DeriveBytes(key, 20);
 
        // Use the password to decrypt the encrypted string
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