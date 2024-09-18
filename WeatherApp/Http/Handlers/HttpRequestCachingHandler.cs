using Polly;

namespace WeatherApp.Http;

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
