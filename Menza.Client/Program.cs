using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Menza.Client;
using Menza.Shared;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("hu-HU");
CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("hu-HU");
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("hu-HU");
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("hu-HU");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<IRepository, Repository>();
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new(builder.HostEnvironment.BaseAddress) });

var app = builder.Build();

await app.RunAsync();