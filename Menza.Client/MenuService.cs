using System.Net.Http.Json;
using Menza.Shared;

namespace Menza.Client;

public class MenuService
{
    private readonly HttpClient _httpClient = new();
    private readonly AuthService _auth;

    public MenuService(AuthService auth) => _auth = auth;

    public async Task<List<MenuAndVotes>> GetAll()
    {
        HttpRequestMessage request = new(HttpMethod.Get, "https://localhost:7181/all");
        if (_auth.AccessToken != null)
            request.Headers.Authorization = new("Bearer", _auth.AccessToken);
        HttpResponseMessage response = await _httpClient.SendAsync(request);
        return (await response.Content.ReadFromJsonAsync<List<MenuAndVotes>>())!;
    }
}