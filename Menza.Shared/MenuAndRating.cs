namespace Menza.Shared;

public record MenuAndRating(DateOnly Date, string Menu, double? Rating, byte? MyRating, int RatingCount)
{
    public double? Rating { get; set; } = Rating;
    public byte? MyRating { get; set; } = MyRating;
    public int RatingCount { get; set; } = RatingCount;
}

public record Rating(DateOnly Date, byte Value);