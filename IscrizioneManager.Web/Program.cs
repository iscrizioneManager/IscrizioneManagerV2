using IscrizioneManager.Core.Services;
using IscrizioneManager.Web;
using IscrizioneManager.Web.Services;
using IscrizioniManager.Controllers;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped<LocalStorageService>();
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<LoginController>();
builder.Services.AddSingleton<ModuloIscrizioneViewModel>();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
var host = builder.Build();

var storage = host.Services.GetRequiredService<LocalStorageService>();

await ClientHolder.EnsureInitializedAsync(storage);

await host.RunAsync();
