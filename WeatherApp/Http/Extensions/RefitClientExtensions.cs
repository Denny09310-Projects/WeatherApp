using Refit;

namespace WeatherApp.Http.Extensions;

public static class RefitClientExtensions
{
    public static IHttpClientBuilder AddApiClient<T>(this IServiceCollection services, string baseAddress) where T : class
    {
        var client = services.AddRefitClient<T>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(baseAddress))
            .AddHttpMessageHandler<ResponseCachingHandler>();

        client.AddStandardResilienceHandler();
        return client;
    }
}
