namespace Menza.Shared;

public record MenuAndRating(DateOnly Date, string Menu, double? Rating, byte? MyVote, int VoteCount);

public record Rating(DateOnly Date, byte Value);