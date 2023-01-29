using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Menza.Server;

public class MenuRepository
{
    private readonly IMemoryCache _cache;
    private readonly Db _db;

    public MenuRepository(IMemoryCache cache, Db db)
    {
        _cache = cache;
        _db = db;
    }

    public async Task<Menu?> GetNext() =>
        await _cache.GetOrCreateAsync(nameof(GetNext), async cacheEntry =>
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            Menu? menu = await _db.Menus
                .OrderBy(m => m.Date)
                .FirstOrDefaultAsync(m => m.Date >= today);
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
        
        foreach (KeyValuePair<int,string?> day in days)
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
        _cache.Remove($"{nameof(GetMonth)}({year}, {month:00})");
    }

    public async Task<List<Menu>> GetMonth(int year, int month) =>
        (await _cache.GetOrCreateAsync($"{nameof(GetMonth)}({year}, {month:00})", async _ =>
        {
            DateOnly firstDayOfMonth = new(year, month, 1);
            DateOnly lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            return await _db.Menus
                .Where(m => m.Date >= firstDayOfMonth && m.Date <= lastDayOfMonth)
                .OrderBy(m => m.Date)
                .ToListAsync();
        }))!;
    
    public async Task<List<Menu>> GetAll() =>
        (await _cache.GetOrCreateAsync(nameof(GetAll), async _ =>
        {
            return await _db.Menus
                .OrderBy(m => m.Date)
                .ToListAsync();
        }))!;
}