using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;

namespace HandleCreateUpdateClientFunction.Utilities
{
    public static class RetryPolicy
    {
        public static AsyncRetryPolicy GetRetryPolicy(ILogger log)
        {
            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(
                    5,
                    _ => TimeSpan.FromMilliseconds(500),
                    (result, timespan, retryNo, context) =>
                    {
                        log.LogError($"{context.OperationKey}: Retry number {retryNo} within " +
                            $"{timespan.TotalMilliseconds}ms. Exception: {result}");
                    }
                );

            return retryPolicy;
        }
    }
}
