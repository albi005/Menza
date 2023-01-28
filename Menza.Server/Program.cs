using Menza.Server;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddDbContext<Db>(o => o.UseSqlite("Data Source=db.sqlite"));
builder.Services.AddMemoryCache();
builder.Services.AddScoped<MenuRepository>();

WebApplication app = builder.Build();

AsyncServiceScope scope = app.Services.CreateAsyncScope();
await scope.ServiceProvider.GetRequiredService<Db>().Database.MigrateAsync();
await scope.DisposeAsync();

app.UseRouting();
app.UseCors();
app.MapGet("/", () => "Hello World!");
app.MapGet("/next", async (MenuRepository menuRepository) => await menuRepository.GetNext())
    .RequireCors(o => o.AllowAnyOrigin());
app.MapControllers();

app.Run();
