using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Serilog;

namespace MyCrmSampleClient.Auth;

public class AuthClient
{
    public async Task<AuthResult> GetAuthorizationToken(AuthConfig authConfig)
    {
        var msg = new HttpRequestMessage {Method = HttpMethod.Post, RequestUri = authConfig.Url};
        var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{authConfig.ClientID}:{authConfig.ClientSecret}"));
        msg.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        msg.Headers.CacheControl = new CacheControlHeaderValue {NoCache = true};
        msg.Content = new FormUrlEncodedContent(new[]
        {
            KeyValuePair.Create("grant_type", "client_credentials"),
            KeyValuePair.Create("scope", authConfig.Scopes ?? "api"),
        });

        using var httpClient = new HttpClient();
        var start = DateTime.Now;
        var resp = await httpClient.SendAsync(msg);
        var body = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode)
        {
            Log.Fatal("Request failed {Status}, {Details}", resp.StatusCode, body);
            return new AuthResult(false, null, DateTime.MinValue);
        }
        else
        {
            var cred = JsonSerializer.Deserialize<ClientCredentialsToken>(body);
            Log.Information("Authorization Credentials {@Cred}", cred);
            return new AuthResult(true, cred.AccessToken, start.AddSeconds(cred.ExpiresIn));
        }
    }
    
    private sealed class ClientCredentialsToken
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
