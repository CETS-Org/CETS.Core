using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Utils.Middleware
{
    /// <summary>
    /// Middleware for rate limiting API requests
    /// </summary>
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private readonly RateLimitOptions _options;

        public RateLimitingMiddleware(
            RequestDelegate next,
            IMemoryCache cache,
            ILogger<RateLimitingMiddleware> logger,
            RateLimitOptions options)
        {
            _next = next;
            _cache = cache;
            _logger = logger;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!_options.Enabled)
            {
                await _next(context);
                return;
            }

            // Get client identifier (IP address or user ID)
            var clientId = GetClientIdentifier(context);
            var endpoint = context.Request.Path.Value ?? "";

            // Check if endpoint should be rate limited
            var policy = GetPolicyForEndpoint(endpoint);
            if (policy == null)
            {
                await _next(context);
                return;
            }

            var key = $"rate_limit:{clientId}:{endpoint}";
            
            if (!_cache.TryGetValue(key, out RequestCounter counter))
            {
                counter = new RequestCounter
                {
                    Count = 0,
                    FirstRequestTime = DateTime.UtcNow
                };
            }

            var timeSinceFirstRequest = DateTime.UtcNow - counter.FirstRequestTime;

            // Reset counter if time window has passed
            if (timeSinceFirstRequest.TotalSeconds >= policy.WindowSeconds)
            {
                counter = new RequestCounter
                {
                    Count = 1,
                    FirstRequestTime = DateTime.UtcNow
                };
            }
            else
            {
                counter.Count++;
            }

            // Check if rate limit exceeded
            if (counter.Count > policy.MaxRequests)
            {
                _logger.LogWarning(
                    "Rate limit exceeded for client {ClientId} on endpoint {Endpoint}. " +
                    "Count: {Count}, Limit: {Limit}, Window: {Window}s",
                    clientId, endpoint, counter.Count, policy.MaxRequests, policy.WindowSeconds);

                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.Headers.Add("Retry-After", policy.WindowSeconds.ToString());
                context.Response.ContentType = "application/json";
                
                var response = new
                {
                    error = "Rate limit exceeded",
                    message = $"Too many requests. Please try again in {policy.WindowSeconds} seconds.",
                    retryAfter = policy.WindowSeconds
                };
                
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            // Update cache
            _cache.Set(key, counter, TimeSpan.FromSeconds(policy.WindowSeconds));

            // Add rate limit headers
            context.Response.Headers.Add("X-RateLimit-Limit", policy.MaxRequests.ToString());
            context.Response.Headers.Add("X-RateLimit-Remaining", (policy.MaxRequests - counter.Count).ToString());
            context.Response.Headers.Add("X-RateLimit-Reset", counter.FirstRequestTime.AddSeconds(policy.WindowSeconds).ToString("o"));

            await _next(context);
        }

        private string GetClientIdentifier(HttpContext context)
        {
            // Try to get user ID from claims
            var userId = context.User?.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "userId")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                return $"user:{userId}";
            }

            // Fall back to IP address
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                ipAddress = forwardedFor.Split(',').First().Trim();
            }

            return $"ip:{ipAddress}";
        }

        private RateLimitPolicy GetPolicyForEndpoint(string endpoint)
        {
            // Check for specific endpoint policies
            foreach (var policy in _options.EndpointPolicies)
            {
                if (endpoint.StartsWith(policy.Key, StringComparison.OrdinalIgnoreCase))
                {
                    return policy.Value;
                }
            }

            // Return default policy
            return _options.DefaultPolicy;
        }

        private class RequestCounter
        {
            public int Count { get; set; }
            public DateTime FirstRequestTime { get; set; }
        }
    }

    public class RateLimitOptions
    {
        public bool Enabled { get; set; } = true;
        public RateLimitPolicy DefaultPolicy { get; set; } = new RateLimitPolicy();
        public Dictionary<string, RateLimitPolicy> EndpointPolicies { get; set; } = new();
    }

    public class RateLimitPolicy
    {
        public int MaxRequests { get; set; } = 100;
        public int WindowSeconds { get; set; } = 60;
    }
}

