using Microsoft.JSInterop;

namespace Menza.Client;

public class AuthService
{
    public AuthService() => Instance = this;

    public string? AccessToken { get; private set; }
    
    private void HandleCredentialCore(string? accessToken)
    {
        Console.WriteLine($"Received access token: {accessToken}");
        AccessToken = accessToken;
        Changed?.Invoke();
    }

    private static AuthService Instance { get; set; } = null!;

    [JSInvokable]
    public static void HandleCredential(string? idToken) => Instance.HandleCredentialCore(idToken);

    public bool IsAuthenticated => AccessToken != null;
    public event Action? Changed;
}