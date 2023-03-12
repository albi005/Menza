namespace Menza.Shared;

public interface IRepository
{
    Task<MenuAndRating?> GetNext();
    Task<List<MenuAndRating>> GetAll();
    Task Rate(Rating rating);
}