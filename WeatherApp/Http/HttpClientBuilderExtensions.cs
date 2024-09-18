using System.Net;
using Polly;
using Polly.Caching;
using Polly.Extensions.Http;

namespace Microsoft.Extensions.DependencyInjection;

internal static class HttpClientBuilderExtensions
{
    internal static IHttpClientBuilder AddDefaultPolicyHandler(this IHttpClientBuilder builder, Action<DefaultPolicyHandlerOptions>? config = null)
    {
        var options = new DefaultPolicyHandlerOptions();
        config?.Invoke(options);

        return builder.AddHttpMessageHandler<HttpRequestCachingHandler>()
            .AddPolicyHandler((sp, req) => RegisterPolicyPipeline(sp, req, options));
    }

    private static IAsyncPolicy<HttpResponseMessage> RegisterPolicyPipeline(IServiceProvider sp, HttpRequestMessage req, DefaultPolicyHandlerOptions options)
    {
        // Bypass policies for POST requests
        if (req.Method == HttpMethod.Post) return Policy.NoOpAsync<HttpResponseMessage>();

        // Retrieve the cache provider from the service provider
        var cache = sp.GetRequiredService<IAsyncCacheProvider>();

        // Define cache expiration strategy based on the HTTP response status code
        var cacheStrategy = new Func<Context, HttpResponseMessage, Ttl>((ctx, result) => new(
            result.StatusCode == HttpStatusCode.OK ? TimeSpan.FromMinutes(5) : TimeSpan.Zero,
            slidingExpiration: true));

        // Create a caching policy using the defined cache provider and expiration strategy
        var cachePolicy = Policy.CacheAsync(
            cacheProvider: cache.AsyncFor<HttpResponseMessage>(),
            ttlStrategy: new ResultTtl<HttpResponseMessage>(cacheStrategy));

        // Define a retry policy for transient HTTP errors with exponential backoff
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

        // Define a circuit breaker policy to handle transient HTTP errors and break the circuit on repeated failures
        var circuitBreakerPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));

        // Combine the caching, retry, and circuit breaker policies into a single policy pipeline
        return Policy.WrapAsync(cachePolicy, retryPolicy, circuitBreakerPolicy);
    }
}

internal class DefaultPolicyHandlerOptions
{
    public TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromMinutes(5);

    public int MaxAttempts { get; set; } = 3;

    public int MaxHandledEventsBeforeBreaking { get; set; } = 3;
}

internal class HttpRequestCachingHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Method == HttpMethod.Get)
        {
            // Create a unique operation key based on the request URI
            var operationKey = request.RequestUri!.AbsoluteUri;

            // Set the policy execution context using the operation key
            var context = new Context(operationKey);
            request.SetPolicyExecutionContext(context);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
