using System.ComponentModel.DataAnnotations;

namespace Menza.Server;

public class EnyCredentials
{
    [Required]
    public string Username { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
}