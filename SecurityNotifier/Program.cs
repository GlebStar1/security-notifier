using SecurityNotifier;
using SecurityNotifier.Services;

var authorizationCredentials = AuthorizationCredentials.LoadSettings();

GraphService.InitializeGraphServiceClient(authorizationCredentials);

Console.WriteLine("Graph client initialized!");
Console.WriteLine("Reading incidents...");

var incidents = await GraphService.GetSecurityIncidentsAsync();

var latestIncidents = incidents?.Value
    ?.Where(x => x.CreatedDateTime > DateTime.Now.AddDays(-1));

Console.WriteLine("Sending incidents...");

var response = await GraphService.SendToTeamChannel(incidents?.Value?.Count);

if (string.IsNullOrEmpty(response?.Body?.Content))
{
    Console.WriteLine("Failed to send to teams channel.");
}

Console.WriteLine("Incidents are sent!");
