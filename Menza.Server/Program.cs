using Menza.Server;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
builder.Services.AddControllers();
builder.Services.AddDbContext<Db>(o => o.UseSqlite("Data Source=db.sqlite"));
builder.Services.AddMemoryCache();
builder.Services.AddScoped<MenuRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpContextAccessor();

WebApplication app = builder.Build();

AsyncServiceScope scope = app.Services.CreateAsyncScope();
await scope.ServiceProvider.GetRequiredService<Db>().Database.MigrateAsync();
await scope.DisposeAsync();

app.UseRouting();
app.UseCors();
app.MapGet("/", () => "Hello World!");
app.MapGet("/next", async (MenuRepository menuRepository) => await menuRepository.GetNext());
app.MapGet("/all", async (AuthService auth, MenuRepository menuRepository) =>
{
    string? email = await auth.Authenticate();
    if (email == null) return await menuRepository.GetAll();
    return await menuRepository.GetAll(email);
});
app.MapPost("/votes", async (AuthService auth, Db db, Menza.Shared.Vote vote) =>
{
    string? email = await auth.Authenticate();
    if (email == null) return Results.Unauthorized();

    Vote? existingVote = await db.Votes.FindAsync(vote.Date, email);
    if (existingVote != null)
    {
        existingVote.Value = vote.Rating;
        db.Votes.Update(existingVote);
    }
    else
        db.Votes.Add(new(vote.Date, email, vote.Rating));
    await db.SaveChangesAsync();
    
    return Results.Ok();
});
app.MapControllers();

app.Run();