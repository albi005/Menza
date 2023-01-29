using Blazored.LocalStorage;
using Microsoft.JSInterop;

namespace Menza.Client;

public class AuthService
{
    private readonly ILocalStorageService _localStorageService;
    
    public AuthService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
        Instance = this;
    }

    public event Action? TokenChanged;
    
    public string? IdToken { get; private set; }
    
    private async void HandleCredentialCore(string idToken)
    {
        IdToken = idToken;
        TokenChanged?.Invoke();
        await _localStorageService.SetItemAsync("idToken", idToken);
    }

    private static AuthService Instance { get; set; } = null!;

    [JSInvokable]
    public static void HandleCredential(string idToken)
    {
        Instance.HandleCredentialCore(idToken);
    }

    public async Task Initialize()
    {
        if (await _localStorageService.ContainKeyAsync("idToken"))
            IdToken = await _localStorageService.GetItemAsync<string>("idToken");
    }
}