using System.Security.Cryptography;
using System.Text;
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
    private string _key;

    private readonly int Keysize = 256;
    private readonly int DerivationIterations = 1000;

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

    public static string EncryptString(string clearText, string encryptionKey)
    {
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    public static string DecryptString(string cipherText, string encryptionKey)
    {
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }
}