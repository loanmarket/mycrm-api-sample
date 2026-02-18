using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MyCrmSampleClient.Auth;
using MyCrmSampleClient.Console;
using MyCrmSampleClient.Kiota;
using MyCrmSampleClient.KiotaExtensions;
using Serilog;

namespace MyCrmSampleClient;

public class Program
{
    static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.SpectreConsole()
            .CreateLogger();

        try
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets(typeof(Program).Assembly, optional: true)
                .AddCommandLine(args)
                .Build();

            var authConfig = config.GetSection("Auth").Get<AuthConfig>();
            var mycrmConfig = config.GetSection("MyCRM").Get<MyCrmConfig>();
            var stateFilePath = GetDefaultStateFilePath();

            if (!ValidateConfig(authConfig, mycrmConfig))
            {
                return;
            }

            var authClient = new AuthClient();
            var token = await authClient.GetAuthorizationToken(authConfig);
            if (!token.Success)
            {
                return;
            }

            var requestAdapter = new HttpClientRequestAdapter(new MyCrmKiotaAuthProvider(token.Token, mycrmConfig.AdviserContactId))
            {
                BaseUrl = mycrmConfig.Url.ToString().TrimEnd('/')
            };

            var mycrmClient = new MyCrmApiClient(requestAdapter);

            JsonApiFluentContext.BaseUrl = requestAdapter.BaseUrl;
            
            var console = new SampleConsole(mycrmClient, mycrmConfig, stateFilePath, authConfig.Scopes);
            
            await console.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Unexpected error");
        }
    }
    
    private static bool ValidateConfig(AuthConfig authConfig, MyCrmConfig mycrmConfig)
    {
        if (authConfig == null || authConfig.Url == null ||
            string.IsNullOrWhiteSpace(authConfig.ClientID) ||
            string.IsNullOrWhiteSpace(authConfig.ClientSecret))
        {
            Log.Fatal("Missing Auth config. Populate Auth:Url, Auth:ClientID and Auth:ClientSecret");
            return false;
        }

        if (mycrmConfig == null || mycrmConfig.Url == null)
        {
            Log.Fatal("Missing MyCRM config. Populate MyCRM:Url");
            return false;
        }

        return true;
    }
    
    private static string GetDefaultStateFilePath()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (string.IsNullOrWhiteSpace(appDataPath))
        {
            appDataPath = Directory.GetCurrentDirectory();
        }

        return Path.Combine(appDataPath, "MyCrmSampleClient", "sample-state.json");
    }
}
