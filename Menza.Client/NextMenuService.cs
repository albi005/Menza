using System.Net.Http.Json;

namespace Menza.Client;

public class NextMenuService
{
    public Menu NextMenu { get; private set; } = null!;

    public async Task LoadNextMenu()
    {
        using HttpClient httpClient = new();
        NextMenu = await httpClient.GetFromJsonAsync<Menu>("https://localhost:7181/next") ?? new(DateOnly.MaxValue);
    }
}