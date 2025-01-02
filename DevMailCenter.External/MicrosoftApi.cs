using System.Net.Http.Json;
using DevMailCenter.External.Models;
using DevMailCenter.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DevMailCenter.External;

public interface IMicrosoftApi
{
    string GenerateAuthenticationRedirectUrl(string redirectUri);
    MicrosoftTokens GetTokensByOnBehalfAccessToken(string accessToken);
    MicrosoftTokens GetTokensByRefreshToken(string refreshToken);
    Task<string> SendEmail(MicrosoftApiMail mail, MicrosoftTokens microsoftTokens);
}

public class MicrosoftApi : IMicrosoftApi
{
    private readonly ILogger<MicrosoftApi> _logger;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public MicrosoftApi(ILogger<MicrosoftApi> logger, IConfiguration configuration, HttpClient httpClient)
    {
        _logger = logger;
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public string GenerateAuthenticationRedirectUrl(string redirectUri)
    {
        var clientId = this._configuration["Microsoft:Authentication:ClientId"];
        var scope = this._configuration["Microsoft:Authentication:Scope"];
        var authority = "https://login.microsoftonline.com/common/";

        string url = $"{authority}/oauth2/v2.0/authorize?";
        url += $"client_id={clientId}";
        url += $"&scope={scope}";
        url += $"&redirect_uri={redirectUri}";
        url += $"&response_mode=fragment";
        url += $"&response_type=token";
        url += $"&prompt=consent";

        return url;
    }

    public MicrosoftTokens GetTokensByOnBehalfAccessToken(string accessToken)
    {
        var clientId = this._configuration["Microsoft:Mailer:ClientId"];
        var clientSecret = this._configuration["Microsoft:Mailer:ClientSecret"];
        var scope = this._configuration["Microsoft:Mailer:Scope"];
        var redirectUri = "http://localhost:4200/callback/microsoft"; // TODO 

        string url = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
        string queryString = $"grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer&client_id={clientId}&client_secret={clientSecret}&assertion={accessToken}&scope={scope}&requested_token_use=on_behalf_of";

        StringContent httpContent = new StringContent(queryString, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
        var res = _httpClient.PostAsync(url, httpContent).Result;
        var deserializedObject = JsonConvert.DeserializeObject<dynamic>(res.Content.ReadAsStringAsync().Result);

        if (deserializedObject.ContainsKey("error"))
        {
            throw new Exception(deserializedObject.error_description.Value);
        }
        else
        {
            var tokens = new MicrosoftTokens()
            {
                AccessToken = deserializedObject.access_token,
                RefreshToken = deserializedObject.refresh_token,
                ExpiresIn = deserializedObject.expires_in
            };
            return tokens;
        }
    }


    public MicrosoftTokens GetTokensByRefreshToken(string refreshToken)
    {
        var clientId = this._configuration["Microsoft:Mailer:ClientId"];
        var clientSecret = this._configuration["Microsoft:Mailer:ClientSecret"];
        var scope = this._configuration["Microsoft:Mailer:Scope"];

        string url = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
        string queryString = $"grant_type=refresh_token&client_id={clientId}&client_secret={clientSecret}&refresh_token={refreshToken}&scope={scope}";

        StringContent httpContent = new StringContent(queryString, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
        var res = _httpClient.PostAsync(url, httpContent).Result;
        var deserializedObject = JsonConvert.DeserializeObject<dynamic>(res.Content.ReadAsStringAsync().Result);

        if (deserializedObject.ContainsKey("error"))
        {
            throw new Exception(deserializedObject.error_description.Value);
        }
        else
        {
            var tokens = new MicrosoftTokens()
            {
                AccessToken = deserializedObject.access_token,
                RefreshToken = deserializedObject.refresh_token,
                ExpiresIn = deserializedObject.expires_in
            };
            return tokens;
        }
    }

    public async Task<string> SendEmail(MicrosoftApiMail mail, MicrosoftTokens microsoftTokens)
    {
        string url = @"https://graph.microsoft.com/v1.0/me/sendMail";
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", microsoftTokens.AccessToken);
        var res = await _httpClient.PostAsJsonAsync(url, mail);
        var content = await res.Content.ReadAsStringAsync();

        if (content != "")
        {
            var deserializedObject = JsonConvert.DeserializeObject<dynamic>(content);

            if (deserializedObject.ContainsKey("error"))
            {
                throw new Exception((string)deserializedObject["error"]["message"]);
            }
            else
            {
                return content;
            }
        }
        else
        {
            return "";
        }
    }
}