using DevMailCenter.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DevMailCenter.External;

public interface IMicrosoftApi
{
    string GenerateAuthenticationRedirectUrl(string redirectUri);
    MicrosoftTokens GetTokensByOnBehalfAccessToken(string accessToken);
    // Task<MicrosoftTokens> GetTokensByRefreshToken(string refreshToken);
    // Task<MicrosoftTokens> GetTokensByRefreshTokenInternal(string refreshToken);
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
        var clientId = this._configuration["Microsoft:ClientId"];
        var scope = this._configuration["Microsoft:Scope"];
        var authority = "https://login.microsoftonline.com/common/";
        
        string url = $"{authority}/oauth2/v2.0/authorize?";
        url += $"client_id={clientId}";
        url += $"&scope=api%3A%2F%2Fnyffels-websites-api%2Faccess_as_user%20api%3A%2F%2Fnyffels-websites-api%2Femail%20api%3A%2F%2Fnyffels-websites-api%2FMail.Send%20api%3A%2F%2Fnyffels-websites-api%2Foffline_access%20api%3A%2F%2Fnyffels-websites-api%2Fopenid%20api%3A%2F%2Fnyffels-websites-api%2Fprofile%20api%3A%2F%2Fnyffels-websites-api%2FUser.Read%20openid%20profile%20offline_access";
        url += $"&redirect_uri={redirectUri}";
        url += $"&response_mode=fragment";
        url += $"&response_type=token";
        url += $"&code_challenge=so0hRl0s-92wTU8QI5Ck9grpsiv_nk93QkREi_qjuAg";
        url += $"&code_challenge_method=S256";
        url += $"&prompt=consent";
        
        return url;
    }
    
    public MicrosoftTokens GetTokensByOnBehalfAccessToken(string accessToken)
    {
        var clientId = this._configuration["Microsoft:ClientId"];
        var clientSecret = this._configuration["Microsoft:ClientSecret"];
        var scope = this._configuration["Microsoft:Scope"];

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

    // public async Task<MicrosoftTokens> GetTokensByRefreshToken(string refreshToken)
    // {
    //     MailTokens accessToken = null;
    //
    //     if (!_cache.TryGetValue<MailTokens>(refreshToken, out accessToken))
    //     {
    //         var tokens = await GetTokensByRefreshTokenInternal(refreshToken);
    //
    //         var options = new MemoryCacheEntryOptions()
    //             .SetAbsoluteExpiration(TimeSpan.FromSeconds(tokens.ExpiresIn - 300));
    //
    //         _cache.Set(refreshToken, tokens);
    //
    //         accessToken = tokens;
    //     }
    //
    //     return accessToken;
    // }
    //
    // private async Task<MicrosoftTokens> GetTokensByRefreshTokenInternal(string refreshToken)
    // {
    //     var clientId = this._configuration["Exchange:GraphApi:ClientId"];
    //     var clientSecret = this._configuration["Exchange:GraphApi:ClientSecret"];
    //     // var scope = this._configuration["Exchange:GraphApi:Scope"];
    //     var scope = "offline_access openid profile email Mail.Send";
    //
    //     const string grant_type = "refresh_token";
    //
    //     string url = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
    //     string queryString = $"grant_type={grant_type}&client_id={clientId}&client_secret={clientSecret}&refresh_token={refreshToken}&scope={scope}";
    //
    //     StringContent httpContent = new StringContent(queryString, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
    //     var res = await this._httpClient.PostAsync(url, httpContent);
    //     var deserializedObject = JsonConvert.DeserializeObject<dynamic>(res.Content.ReadAsStringAsync().Result);
    //
    //     if (deserializedObject.ContainsKey("error"))
    //     {
    //         throw new Exception(deserializedObject.error_description.Value);
    //     }
    //     else
    //     {
    //         var tokens = new MailTokens()
    //         {
    //             AccessToken = deserializedObject.access_token,
    //             RefreshToken = deserializedObject.refresh_token,
    //             ExpiresIn = deserializedObject.expires_in
    //         };
    //         return tokens;
    //     }
    // }
}