using Google.Apis.Auth;

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
            CustomPayload payload = await JsonWebSignature.VerifySignedTokenAsync<CustomPayload>(
                idToken,
                new() { ExpiryClockTolerance = TimeSpan.FromDays(100) });
            return !payload.Email.EndsWith("eotvos-tata.org")
                ? null
                : payload.Email;
        }
        catch (InvalidJwtException)
        {
            return null;
        }
    }
}