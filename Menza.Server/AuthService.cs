using System.Diagnostics;
using FirebaseAdmin.Auth;

namespace Menza.Server;

public class AuthService
{
    private readonly HttpContext _context;

    public AuthService(IHttpContextAccessor accessor) => _context = accessor.HttpContext!;

    public async Task<string?> Authenticate()
    {
        if (!_context.Request.Headers.Authorization.Any()) return null;
        string header = _context.Request.Headers.Authorization!;
        if (!header.StartsWith("Bearer ")) return null;
        string idToken = header.Split(' ')[1];

        try
        {
            if (FirebaseAuth.DefaultInstance == null) return null;
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
            Debug.WriteLine(decodedToken.Uid);
            string? email = decodedToken.Claims["email"]?.ToString();
            return email?.EndsWith("@eotvos-tata.org") != true ? null : email;
        }
        catch (FirebaseAuthException)
        {
            return null;
        }
    }
}