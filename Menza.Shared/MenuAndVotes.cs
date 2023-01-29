namespace Menza.Shared;

public record MenuAndVotes(DateOnly Date, string Menu, float? Rating, float? MyVote, int VoteCount)
{
    public float? MyVote { get; set; } = MyVote;
}

public record Vote(DateOnly Date, float Rating);