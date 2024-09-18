namespace Microsoft.Extensions.DependencyInjection;

internal class DefaultPolicyHandlerOptions
{
    public TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromMinutes(5);

    public int MaxAttempts { get; set; } = 3;

    public int MaxHandledEventsBeforeBreaking { get; set; } = 3;
}
