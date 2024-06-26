using System.Globalization;
using System.Text;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Menza.Client;
using Menza.Server;
using Menza.Shared;
using Microsoft.EntityFrameworkCore;
using AuthService = Menza.Server.AuthService;
using Repository = Menza.Server.Repository;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("hu");

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<EnyCredentials>()
    .Bind(builder.Configuration.GetSection("EnyCredentials"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddDbContext<Db>(o => o.UseSqlite(
    builder.Configuration.GetConnectionString("Database") ?? throw new("Database connection string is not set")));
builder.Services.AddMemoryCache();
builder.Services.AddScoped<Repository>();
builder.Services.AddScoped<IRepository>(sp => sp.GetRequiredService<Repository>());
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<UpdateService>();

CultureInfo.DefaultThreadCurrentCulture = new("hu-HU");
CultureInfo.DefaultThreadCurrentUICulture = new("hu-HU");

WebApplication app = builder.Build();

string? serviceAccount = app.Configuration["Firebase:ServiceAccount"];
if (serviceAccount != null)
{
    FirebaseApp.Create(new AppOptions
    {
        Credential = serviceAccount.StartsWith('{')
            ? GoogleCredential.FromJson(serviceAccount)
            : GoogleCredential.FromFile(serviceAccount),
    });
}

AsyncServiceScope scope = app.Services.CreateAsyncScope();
await scope.ServiceProvider.GetRequiredService<Db>().Database.MigrateAsync();
await scope.DisposeAsync();

if (app.Environment.IsDevelopment())
    app.UseWebAssemblyDebugging();
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<Menza.Server.Components.Index>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(App).Assembly);

app.MapGet("/next", async (Repository repository) => await repository.GetNext());
app.MapGet("/all", async (Repository repository) => await repository.GetAll());
app.MapPost("/votes", async (Rating rating, Repository repository) => await repository.Rate(rating));
app.MapControllers();
app.MapGet("/shadows", () =>
{
    StringBuilder response = new();
    for (int i = 0; i <= 24; i++)
        response.AppendLine($"--shadow-{i}: {Shadow.ComputeCssShadow(i)};");
    return response.ToString();
});

app.Run();