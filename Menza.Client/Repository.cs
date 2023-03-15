using System.Net.Http.Json;
using Menza.Shared;

namespace Menza.Client;

public class Repository : IRepository
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _auth;

    public Repository(HttpClient httpClient, AuthService auth)
    {
        _httpClient = httpClient;
        _auth = auth;
    }

    public async Task<MenuAndRating?> GetNext() => await Get<MenuAndRating?>("next");
    public async Task<List<MenuAndRating>> GetAll() => await Get<List<MenuAndRating>>("all");

    public async Task Rate(Rating rating)
    {
        if (_auth.AccessToken == null) throw new("Not authenticated");
        HttpRequestMessage request = new(HttpMethod.Post, "votes");
        request.Content = JsonContent.Create(rating);
        request.Headers.Authorization = new("Bearer", _auth.AccessToken);
        await _httpClient.SendAsync(request);
    }

    private async Task<T> Get<T>(string path)
    {
        Console.WriteLine($"Getting {path}");
        HttpRequestMessage request = new(HttpMethod.Get, path);
        if (_auth.AccessToken != null)
            request.Headers.Authorization = new("Bearer", _auth.AccessToken);
        
        HttpResponseMessage response = await _httpClient.SendAsync(request);
        T? result = await response.Content.ReadFromJsonAsync<T>();
        return result!;
    }
}