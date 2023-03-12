using Menza.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Menza.Server;

public class Repository : IRepository
{
    private readonly IMemoryCache _cache;
    private readonly Db _db;
    private readonly AuthService _auth;

    public Repository(IMemoryCache cache, Db db, AuthService auth)
    {
        _cache = cache;
        _db = db;
        _auth = auth;
    }

    public async Task<MenuAndRating?> GetNext() =>
        await _cache.GetOrCreateAsync(nameof(GetNext), async cacheEntry =>
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            MenuAndRating? menu = await _db.Menus
                .Where(m => m.Date >= today)
                .OrderBy(m => m.Date)
                .Select(m => new MenuAndRating(
                    m.Date,
                    m.Value,
                    m.Votes != null ? m.Votes.Average(v => v.Value) : null,
                    null,
                    m.Votes != null ? m.Votes.Count : 0))
                .FirstOrDefaultAsync();
            if (menu != null)
                cacheEntry.SetAbsoluteExpiration(menu.Date.ToDateTime(default).AddDays(1));
            return menu;
        });

    public async Task UpdateMonth(int year, int month, Dictionary<int, string?> days)
    {
        if (!days.Any()) return;

        DateOnly firstDayOfMonth = new(year, month, 1);
        DateOnly lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        Dictionary<int, Menu> menus = await _db.Menus
            .Where(m => m.Date >= firstDayOfMonth && m.Date <= lastDayOfMonth)
            .OrderBy(m => m.Date)
            .ToDictionaryAsync(m => m.Date.Day);

        foreach (KeyValuePair<int, string?> day in days)
        {
            if (menus.TryGetValue(day.Key, out var menu))
            {
                if (day.Value == null)
                    _db.Remove(menu);
                else
                    menu.Value = day.Value;
            }
            else if (day.Value != null)
                _db.Menus.Add(new(new(year, month, day.Key), day.Value));
        }

        await _db.SaveChangesAsync();

        _cache.Remove(nameof(GetNext));
        _cache.Remove(nameof(GetAll));
    }

    public async Task<List<MenuAndRating>> GetAll()
    {
        string? email = await _auth.Authenticate();
        if (email == null)
            return (await _cache.GetOrCreateAsync(nameof(GetAll), async _ =>
            {
                return await _db.Menus
                    .OrderBy(m => m.Date)
                    .Select(m => new MenuAndRating(
                        m.Date,
                        m.Value,
                        m.Votes != null ? m.Votes.Average(v => v.Value) : null,
                        null,
                        m.Votes!.Count))
                    .ToListAsync();
            }))!;
        
        return await _db.Menus
            .OrderBy(m => m.Date)
            .Select(m => new MenuAndRating(
                m.Date,
                m.Value,
                m.Votes != null ? m.Votes.Average(v => v.Value) : null,
                m.Votes!.Any(v => v.Email == email) ? m.Votes!.First(v => v.Email == email).Value : null,
                m.Votes!.Count))
            .ToListAsync();
    }

    public async Task Rate(Rating rating)
    {
        string? email = await _auth.Authenticate();
        if (email == null) return;

        Vote? existingVote = await _db.Votes.FindAsync(rating.Date, email);
        if (existingVote != null)
            existingVote.Value = rating.Value;
        else
            _db.Votes.Add(new(rating.Date, email, rating.Value));
        await _db.SaveChangesAsync();

        _cache.Remove(nameof(GetAll));
        _cache.Remove(nameof(GetNext));
    }
}