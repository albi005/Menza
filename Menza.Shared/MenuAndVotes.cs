namespace Menza.Shared;

public record MenuAndVotes(DateOnly Date, string Menu, float Rating, byte? MyVote)
{
    public byte? MyVote { get; set; } = MyVote;
}