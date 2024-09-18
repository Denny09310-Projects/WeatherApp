using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Polly.Caching;
using Polly.Caching.Memory;
using Refit;
using WeatherApp;
using WeatherApp.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMemoryCache();

builder.Services.AddRefitClient<IWeatherClient>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://api.open-meteo.com/v1"))
    .AddDefaultPolicyHandler(config => config.CacheExpirationTime = TimeSpan.FromHours(1));

builder.Services.AddScoped<HttpRequestCachingHandler>();

builder.Services.AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();

await builder.Build().RunAsync();
