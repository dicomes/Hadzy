using System.Net;
using Google;
using Polly;
using Polly.Retry;

namespace YouTubeCommentsFetcher.Worker.Services;

public class RetryPolicyProvider
{
    /// <summary>
    /// 
    /// This policy is configured to handle two types of exceptions:
    /// 1. GoogleApiException where the HTTP status code is 'InternalServerError'.
    ///    This typically indicates a transient error on the server's end.
    /// 2. TimeoutException which indicates the request took too long.
    /// 
    /// In the event one of these exceptions is encountered, the operation will be retried up to 3 times.
    /// If the exception persists after 3 retries, it will be thrown.
    /// 
    /// For all other exceptions not specified in this policy, they will propagate immediately without any retries.
    /// </summary>
    public AsyncRetryPolicy GetYouTubeApiRetryPolicy()
    {
        return Policy
            .Handle<GoogleApiException>(e => e.HttpStatusCode == HttpStatusCode.InternalServerError)
            .Or<TimeoutException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}