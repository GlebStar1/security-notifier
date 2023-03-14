using Microsoft.Extensions.Configuration;

namespace SecurityNotifier;

public class TeamsConfiguration
{
    public string ChannelId { get; set; }
    public string TeamId { get; set; }

    public static TeamsConfiguration LoadSettings()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<Program>()
            .Build();

        return config.GetRequiredSection("TeamsConfiguration").Get<TeamsConfiguration>() ??
               throw new Exception("Could not load app settings. See README for configuration instructions.");
    }
}