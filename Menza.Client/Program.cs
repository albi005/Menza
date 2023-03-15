using Blazored.LocalStorage;
using Menza.Client;
using Menza.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<IRepository, Repository>();
builder.Services.AddSingleton<StartupService>();
builder.Services.AddSingleton(_ => new HttpClient { BaseAddress = new(builder.HostEnvironment.BaseAddress) });

var app = builder.Build();

await app.Services.GetRequiredService<AuthService>().Initialize();
await app.Services.GetRequiredService<StartupService>().Initialize();

await app.RunAsync();