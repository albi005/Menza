using System.Globalization;
using Blazored.LocalStorage;
using Menza.Client;
using Menza.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<IRepository, Repository>();
builder.Services.AddSingleton<StartupService>();
builder.Services.AddSingleton(_ => new HttpClient { BaseAddress = new(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<CardService>();

CultureInfo.DefaultThreadCurrentCulture = new("hu-HU");
CultureInfo.DefaultThreadCurrentUICulture = new("hu-HU");

var app = builder.Build();

await app.Services.GetRequiredService<AuthService>().Initialize();
await app.Services.GetRequiredService<StartupService>().Initialize();


await app.RunAsync();