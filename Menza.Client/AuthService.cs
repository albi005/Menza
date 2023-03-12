using Microsoft.JSInterop;

namespace Menza.Client;

public class AuthService
{
    public AuthService()
    {
        Instance = this;
    }

    public event Action? TokenChanged;
    
    public string? AccessToken { get; private set; }
    
    private void HandleCredentialCore(string? accessToken)
    {
        Console.WriteLine($"Received access token: {accessToken}");
        AccessToken = accessToken;
        TokenChanged?.Invoke();
    }

    private static AuthService Instance { get; set; } = null!;

    [JSInvokable]
    public static void HandleCredential(string? idToken) => Instance.HandleCredentialCore(idToken);
}