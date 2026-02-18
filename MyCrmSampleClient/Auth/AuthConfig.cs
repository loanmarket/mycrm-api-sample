using System;

namespace MyCrmSampleClient.Auth;

public class AuthConfig
{
    public Uri Url { get; set; }
    public string ClientID { get; set; }
    public string ClientSecret { get; set; }
    public string Scopes { get; set; }
}
