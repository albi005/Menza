using System.Runtime.InteropServices.JavaScript;

namespace Menza.Client;

public partial class CredentialService
{
    public CredentialService() => Instance = this;

    public string? IdToken { get; private set; }
    
    private static CredentialService? Instance { get; set; }

    [JSExport]
    public static void HandleCredential(string idToken) => Instance!.IdToken = idToken;
}