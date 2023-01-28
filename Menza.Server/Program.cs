using Menza.Server;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MenuRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddCors();

WebApplication app = builder.Build();

app.UseRouting();
app.UseCors();

app.MapUpdater();
app.MapGet("/", () => "Hello World!");
app.MapGet("/next", async (MenuRepository menuRepository) => await menuRepository.GetNext())
    .RequireCors(o => o.AllowAnyOrigin());

app.Run();
