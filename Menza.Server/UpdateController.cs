using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace Menza.Server;

[ApiController]
[Route("/update")]
public class UpdateController : ControllerBase
{
    private readonly Repository _repository;

    public UpdateController(Repository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task Update([FromQuery] string sessionId)
    {
        HttpClient httpClient = new();

        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        for (int i = -1; i <= 1; i++)
        {
            DateOnly date = today.AddMonths(i);

            HttpRequestMessage httpRequestMessage =
                new(HttpMethod.Post, "https://tata.eny.hu/index.php?m=directapi");
            httpRequestMessage.Headers.Add("X-Requested-With", "XMLHttpRequest");
            httpRequestMessage.Headers.Add("Cookie", $"PHPSESSID={sessionId}");
            httpRequestMessage.Content = new StringContent(
                $$"""{"action":"studentorder","method":"loadOrders","tid":1,"type":"rpc","data":["2867","{{date.Year}}-{{date.Month:00}}-01"]}""");

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            if (!httpResponseMessage.IsSuccessStatusCode) continue;

            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            IEnumerable<KeyValuePair<string, JsonNode?>>? jsonDays =
                JsonNode.Parse(content)?["result"]?["dailymenu"] as JsonObject;

            if (jsonDays?.Any() != true) continue;

            Dictionary<int, string?> days = jsonDays
                .ToDictionary(x => int.Parse(x.Key), x => x.Value!["19"]?.GetValue<string>());

            await _repository.UpdateMonth(date.Year, date.Month, days);
        }
    }
}