using Polly.Caching;
using Polly.Caching.Memory;
using WeatherApp.Http;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddHttpCache(this IServiceCollection services)
    {
        return services.AddMemoryCache()
            .AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>()
            .AddScoped<HttpRequestCachingHandler>();
    }
}
