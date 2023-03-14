using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.Security;

namespace SecurityNotifier.Services;

public class GraphService
{
    private static AuthorizationCredentials? _authorizationCredentials;
    private static ClientSecretCredential? _clientSecretCredential;
    private static GraphServiceClient? _graphServiceClient;
    private static TeamsConfiguration _teamsConfiguration;

    public static void InitializeGraphServiceClient(AuthorizationCredentials authorizationCredentials)
    {
        _ = authorizationCredentials ??
            throw new NullReferenceException("Settings cannot be null");
        
        _authorizationCredentials = authorizationCredentials;

        if (_clientSecretCredential == null)
        {
            _clientSecretCredential = new ClientSecretCredential(
                _authorizationCredentials.TenantId, 
                _authorizationCredentials.ClientId, 
                _authorizationCredentials.ClientSecret);
        }

        if (_graphServiceClient == null)
        {
            _graphServiceClient = new GraphServiceClient(_clientSecretCredential,
                // Use the default scope, which will request the scopes
                // configured on the app registration
                new[] {"https://graph.microsoft.com/.default"});
        }
    }

    public static async Task<IncidentCollectionResponse?> GetSecurityIncidentsAsync()
    {
        return await _graphServiceClient.Security.Incidents.GetAsync();
    }
    
    public static async Task<ChatMessage?> SendToTeamChannel(int? incidentsCount)
    {
        var requestBody = new ChatMessage
        {
            Body = new ItemBody
            {
                Content = $"There was {incidentsCount} incidents for last 24 hours."
            }
        };
        
        return await _graphServiceClient.Teams[$"{_teamsConfiguration.TeamId}"]
            .Channels[$"{_teamsConfiguration.ChannelId}"].Messages.PostAsync(requestBody);
    }
}