using System.Text.Json.Serialization;
using Google.Apis.Auth;

public class CustomPayload : JsonWebSignature.Payload
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;
}