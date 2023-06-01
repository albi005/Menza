using Microsoft.JSInterop;

namespace Menza.Client;

public class AuthService
{
    private readonly IJSRuntime _jsRuntime;
    
    public AuthService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public string? AccessToken { get; private set; }
    
    public async Task Initialize()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("auth.registerCredentialHandler", DotNetObjectReference.Create(this));
            AccessToken = await _jsRuntime.InvokeAsync<string?>("auth.getAccessToken");
        }
        catch { /*_*/ }
    }

    [JSInvokable]
    public void HandleCredential(string? accessToken)
    {
        Console.WriteLine($"Received access token: {accessToken?[..24]}");
        AccessToken = accessToken;
        Changed?.Invoke();
    }

    public bool IsAuthenticated => AccessToken != null;
    public event Action? Changed;
}