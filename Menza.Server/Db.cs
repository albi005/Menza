using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Menza.Server;

public class Db : DbContext
{
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<Vote> Votes => Set<Vote>();

    public Db(DbContextOptions<Db> options) : base(options)
    {
    }
}

[PrimaryKey(nameof(Date))]
public record Menu(DateOnly Date, string Value)
{
    public string Value { get; set; } = Value;
    public List<Vote>? Votes { get; set; }
}

[PrimaryKey(nameof(Date), nameof(Email))]
public record Vote(DateOnly Date, string Email, float Value)
{
    public float Value { get; set; } = Value;
    [ForeignKey(nameof(Date))] public Menu Menu { get; set; } = null!;
}