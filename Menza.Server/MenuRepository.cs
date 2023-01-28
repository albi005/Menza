using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Menza.Server;

public class MenuRepository
{
    private readonly IMemoryCache _cache;
    private readonly Task _initialize;
    private readonly ConcurrentDictionary<DateOnly, Dictionary<int, string>> _store = new();

    public MenuRepository(IMemoryCache cache)
    {
        _cache = cache;
        _initialize = Initialize();
    }

    public async Task<string?> GetNext()
    {
        await _initialize;

        return _cache.GetOrCreate("nextMenu", cacheEntry =>
        {
            DateTime now = DateTime.Now;
            DateOnly thisMonth = new(now.Year, now.Month, 1);

            string? todaysMenu = _store.GetValueOrDefault(thisMonth)?.GetValueOrDefault(now.Day);
            if (todaysMenu != null) return todaysMenu;

            foreach (var month in _store.Where(x => x.Key >= thisMonth).OrderBy(x => x.Key))
            {
                foreach (var day in month.Value)
                {
                    if (month.Key == thisMonth && day.Key < now.Day) continue;

                    cacheEntry.SetAbsoluteExpiration(
                        new DateTimeOffset(
                            month.Key.Year, month.Key.Month, day.Key, 0, 0, 0, default));
                    return day.Value;
                }
            }

            return null;
        });
    }

    public async Task UpdateMonth(int year, int month, Dictionary<int, string> days)
    {
        await _initialize;
        _store[new(year, month, 1)] = days;
        _cache.Remove("nextMenu");
        await File.WriteAllTextAsync($"menu{year}{month:00}.json", JsonSerializer.Serialize(days));
    }

    public async Task<Dictionary<int, string>?> GetMonth(int year, int month)
    {
        await _initialize;
        return _store.GetValueOrDefault(new(year, month, 1));
    }

    private async Task Initialize()
    {
        foreach (MonthEntry entry in await Task.WhenAll(Directory
                     .GetFiles(".", "menu??????.json")
                     .Select(async s =>
                     {
                         string json = await File.ReadAllTextAsync(s);
                         string fileName = Path.GetFileName(s);
                         return new MonthEntry(
                             new(
                                 int.Parse(fileName[4..^7]),
                                 int.Parse(fileName[8..^5]),
                                 1),
                             JsonSerializer.Deserialize<Dictionary<int, string>>(json)!);
                     })))
        {
            _store[entry.Month] = entry.Days;
        }
    }
}

public record MonthEntry(DateOnly Month, Dictionary<int, string> Days);