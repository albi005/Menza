using System.Net.Http.Json;
using Menza.Shared;

namespace Menza.Client;

public class VoteService
{
    private readonly AuthService _authService;
    private readonly HttpClient _httpClient = new();

    public VoteService(AuthService authService)
    {
        _authService = authService;
    }
    
    public async Task Vote(Vote vote)
    {
        if (_authService.AccessToken == null)
            throw new InvalidOperationException("Not logged in");
        HttpRequestMessage request = new(HttpMethod.Post, "https://localhost:7181/votes");
        request.Headers.Authorization = new("Bearer",_authService.AccessToken);
        request.Content = JsonContent.Create(vote);
        await _httpClient.SendAsync(request);
    }
}