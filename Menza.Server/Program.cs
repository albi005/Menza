using System.Globalization;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Menza.Server;
using Menza.Shared;
using Microsoft.EntityFrameworkCore;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("hu");

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<Db>(o => o.UseSqlite("Data Source=db.sqlite"));
builder.Services.AddMemoryCache();
builder.Services.AddScoped<Repository>();
builder.Services.AddScoped<IRepository>(sp => sp.GetRequiredService<Repository>());
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpContextAccessor();

WebApplication app = builder.Build();

string serviceAccount = app.Configuration["Firebase:ServiceAccount"] ?? throw new("Firebase:ServiceAccount is not set");
FirebaseApp.Create(new AppOptions
{
    Credential = serviceAccount.StartsWith('{')
        ? GoogleCredential.FromJson(serviceAccount)
        : GoogleCredential.FromFile(serviceAccount),
});

AsyncServiceScope scope = app.Services.CreateAsyncScope();
await scope.ServiceProvider.GetRequiredService<Db>().Database.MigrateAsync();
await scope.DisposeAsync();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseCors();
app.MapGet("/next", async (Repository repository) => await repository.GetNext());
app.MapGet("/all", async (Repository repository) => await repository.GetAll());
app.MapPost("/votes", async (Rating rating, Repository repository) => await repository.Rate(rating));
app.MapRazorPages();
app.MapControllers();

app.Run();