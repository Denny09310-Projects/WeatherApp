using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMemoryCache();
builder.Services.AddScoped<ResponseCachingHandler>();

builder.Services.AddLocalStorageServices();
builder.Services.AddSingleton<ILocalStorageCache, LocalStorageCache>();

builder.Services.AddApiClient<IWeatherApi>("https://api.open-meteo.com/v1");
builder.Services.AddApiClient<IGeocodingApi>("https://geocoding-api.open-meteo.com/v1");

await builder.Build().RunAsync();
