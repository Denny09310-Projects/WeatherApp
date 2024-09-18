namespace WeatherApp.Http.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class ResponseCachingAttribute(TimeSpan expirationTime) : Attribute
{
    public TimeSpan ExpirationTime { get; set; } = expirationTime;

    public ResponseCachingAttribute(int seconds) : this(TimeSpan.FromSeconds(seconds))
    {
    }
}