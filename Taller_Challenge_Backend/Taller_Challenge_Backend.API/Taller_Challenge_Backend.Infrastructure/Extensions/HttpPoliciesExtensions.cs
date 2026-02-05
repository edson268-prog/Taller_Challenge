using Polly;
using Polly.Extensions.Http;
using System.Net;

namespace Taller_Challenge_Backend.Infrastructure.Extensions
{
    public static class HttpPoliciesExtensions
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
