using Blazored.LocalStorage;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.JSInterop;

namespace Menza.Client;

public class AuthService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IJSRuntime _jsRuntime;
    
    public AuthService(ILocalStorageService localStorageService, IJSRuntime jsRuntime)
    {
        _localStorageService = localStorageService;
        _jsRuntime = jsRuntime;
        Instance = this;
    }

    public event Action? TokenChanged;
    
    public string? IdToken { get; private set; }
    
    private async void HandleCredentialCore(string idToken)
    {
        JsonWebToken payload = new(idToken);
        if (payload.Claims.FirstOrDefault(c => c.Type == "email")?.Value.EndsWith("eotvos-tata.org") != true)
        {
            await _jsRuntime.InvokeVoidAsync("alert", "Az eotvos-tata.org végű emailedet használd!", "Asd");
            return;
        }
        IdToken = idToken;
        TokenChanged?.Invoke();
        await _localStorageService.SetItemAsync("idToken", idToken);
    }

    private static AuthService Instance { get; set; } = null!;

    [JSInvokable]
    public static void HandleCredential(string idToken) => Instance.HandleCredentialCore(idToken);

    public async Task Initialize()
    {
        if (await _localStorageService.ContainKeyAsync("idToken"))
            IdToken = await _localStorageService.GetItemAsync<string>("idToken");
    }
}