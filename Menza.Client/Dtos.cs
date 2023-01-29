namespace Menza.Client;

public record Vote(DateOnly Date, string Email, float Value);

public record Menu(DateOnly Date, string Value);