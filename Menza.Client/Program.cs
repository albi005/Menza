using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Menza.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<NextMenuService>();
builder.Services.AddSingleton<VoteService>();
builder.Services.AddSingleton<MenuService>();
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new(builder.HostEnvironment.BaseAddress) });

var app = builder.Build();

await app.Services.GetRequiredService<NextMenuService>().LoadNextMenu();

await app.RunAsync();