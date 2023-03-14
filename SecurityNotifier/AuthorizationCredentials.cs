using Microsoft.Extensions.Configuration;

namespace SecurityNotifier;

public class AuthorizationCredentials
{
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? TenantId { get; set; }

    public static AuthorizationCredentials LoadSettings()
    {
        
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<Program>()
            .Build();

        return config.GetRequiredSection("AuthorizationCredentials").Get<AuthorizationCredentials>() ??
               throw new Exception("Could not load app settings. See README for configuration instructions.");
    }
}