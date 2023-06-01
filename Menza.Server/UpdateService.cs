using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;

namespace Menza.Server;

public class UpdateService : BackgroundService
{
    private readonly EnyCredentials _credentials;
    private readonly IServiceProvider _serviceProvider;
    
    public UpdateService(IOptions<EnyCredentials> credentials, IServiceProvider serviceProvider)
    {
        _credentials = credentials.Value;
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // every day at 05:00
        return Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Update(stoppingToken);
                }
                catch { /*_*/ }

                DateTime now = DateTime.Now;
                DateTime next = now.Date.AddDays(1).AddHours(5);
                TimeSpan delay = next - now;
                await Task.Delay(delay, stoppingToken);
            }
        }, stoppingToken);
    }

    private async Task Update(CancellationToken ct)
    {
        HttpClient httpClient = new();
        
        HttpRequestMessage request = new(
            HttpMethod.Post,
            "https://tata.eny.hu/");
        Dictionary<string, string> parameters = new()
        {
            ["name"] = _credentials.Username,
            ["password"] = _credentials.Password,
        };
        request.Content = new FormUrlEncodedContent(parameters);
        await httpClient.SendAsync(request, ct);

        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        for (int i = -1; i <= 1; i++)
        {
            DateOnly date = today.AddMonths(i);

            HttpRequestMessage httpRequestMessage =
                new(HttpMethod.Post, "https://tata.eny.hu/index.php?m=directapi");
            httpRequestMessage.Headers.Add("X-Requested-With", "XMLHttpRequest");
            httpRequestMessage.Content = new StringContent(
                $$"""{"action":"studentorder","method":"loadOrders","tid":1,"type":"rpc","data":["2867","{{date.Year}}-{{date.Month:00}}-01"]}""");

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, ct);
            if (!httpResponseMessage.IsSuccessStatusCode) continue;

            string content = await httpResponseMessage.Content.ReadAsStringAsync(ct);

            IEnumerable<KeyValuePair<string, JsonNode?>>? jsonDays =
                JsonNode.Parse(content)?["result"]?["dailymenu"] as JsonObject;

            if (jsonDays?.Any() != true) continue;

            Dictionary<int, string?> days = jsonDays
                .ToDictionary(x => int.Parse(x.Key), x => x.Value!["19"]?.GetValue<string>());

            ct.ThrowIfCancellationRequested();
            
            using IServiceScope scope = _serviceProvider.CreateScope();
            Repository repository = scope.ServiceProvider.GetRequiredService<Repository>();
            await repository.UpdateMonth(date.Year, date.Month, days);
        }
    }
}