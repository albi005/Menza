namespace Menza.Shared;

public record MenuAndVotes(DateOnly Date, string Menu, double? Rating, byte? MyVote, int VoteCount);

public record Vote(DateOnly Date, byte Rating);