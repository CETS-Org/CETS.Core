using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace Utils.Middleware
{
    /// <summary>
    /// Middleware for validating and sanitizing incoming requests
    /// </summary>
    public class RequestValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestValidationMiddleware> _logger;
        private readonly RequestValidationOptions _options;

        // Common malicious patterns
        private static readonly Regex SqlInjectionPattern = new Regex(
            @"(\bOR\b|\bAND\b).*(=|LIKE|IN)\s*['""]|union.*select|insert\s+into|delete\s+from|drop\s+table|exec(\s|\()|script.*>|<.*script",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex XssPattern = new Regex(
            @"<script.*?>.*?</script>|javascript:|onerror=|onload=|<iframe|eval\(|alert\(",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex PathTraversalPattern = new Regex(
            @"\.\./|\.\.\\|%2e%2e[/\\]",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public RequestValidationMiddleware(
            RequestDelegate next,
            ILogger<RequestValidationMiddleware> logger,
            RequestValidationOptions options)
        {
            _next = next;
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

            // Validate request size
            if (context.Request.ContentLength > _options.MaxRequestBodySize)
            {
                _logger.LogWarning("Request body size exceeds limit: {Size} bytes", context.Request.ContentLength);
                context.Response.StatusCode = (int)HttpStatusCode.RequestEntityTooLarge;
                context.Response.ContentType = "application/json";
                
                var response = new
                {
                    error = "Request too large",
                    message = $"Request body must not exceed {_options.MaxRequestBodySize} bytes"
                };
                
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            // Validate query parameters
            if (_options.ValidateQueryString && context.Request.QueryString.HasValue)
            {
                var queryString = context.Request.QueryString.Value ?? "";
                if (ContainsMaliciousContent(queryString, out var threatType))
                {
                    _logger.LogWarning(
                        "Malicious content detected in query string. Type: {ThreatType}, IP: {IP}, Path: {Path}",
                        threatType, context.Connection.RemoteIpAddress, context.Request.Path);

                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    
                    var response = new
                    {
                        error = "Invalid request",
                        message = "The request contains potentially malicious content"
                    };
                    
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    return;
                }
            }

            // Validate headers
            if (_options.ValidateHeaders)
            {
                foreach (var header in context.Request.Headers)
                {
                    if (ContainsMaliciousContent(header.Value.ToString(), out var threatType))
                    {
                        _logger.LogWarning(
                            "Malicious content detected in header {HeaderName}. Type: {ThreatType}, IP: {IP}",
                            header.Key, threatType, context.Connection.RemoteIpAddress);

                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json";
                        
                        var response = new
                        {
                            error = "Invalid request",
                            message = "The request headers contain potentially malicious content"
                        };
                        
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        return;
                    }
                }
            }

            // Validate User-Agent
            if (_options.RequireUserAgent && string.IsNullOrWhiteSpace(context.Request.Headers["User-Agent"]))
            {
                _logger.LogWarning("Request without User-Agent from IP: {IP}", context.Connection.RemoteIpAddress);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                
                var response = new
                {
                    error = "Invalid request",
                    message = "User-Agent header is required"
                };
                
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }

            await _next(context);
        }

        private bool ContainsMaliciousContent(string input, out string threatType)
        {
            if (string.IsNullOrEmpty(input))
            {
                threatType = null;
                return false;
            }

            if (SqlInjectionPattern.IsMatch(input))
            {
                threatType = "SQL Injection";
                return true;
            }

            if (XssPattern.IsMatch(input))
            {
                threatType = "XSS Attack";
                return true;
            }

            if (PathTraversalPattern.IsMatch(input))
            {
                threatType = "Path Traversal";
                return true;
            }

            threatType = null;
            return false;
        }
    }

    public class RequestValidationOptions
    {
        public bool Enabled { get; set; } = true;
        public long MaxRequestBodySize { get; set; } = 10 * 1024 * 1024; // 10 MB
        public bool ValidateQueryString { get; set; } = true;
        public bool ValidateHeaders { get; set; } = true;
        public bool RequireUserAgent { get; set; } = false;
    }
}

