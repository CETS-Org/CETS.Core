using Microsoft.AspNetCore.Http;

namespace Utils.Middleware
{
    /// <summary>
    /// Middleware to add security headers to responses
    /// </summary>
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // X-Content-Type-Options: Prevents MIME type sniffing
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

            // X-Frame-Options: Prevents clickjacking attacks
            context.Response.Headers.Add("X-Frame-Options", "DENY");

            // X-XSS-Protection: Enables XSS filter in older browsers
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

            // Referrer-Policy: Controls referrer information
            context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");

            // Permissions-Policy: Controls browser features
            context.Response.Headers.Add("Permissions-Policy", 
                "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");

            // Content-Security-Policy: Prevents XSS and data injection attacks
            context.Response.Headers.Add("Content-Security-Policy",
                "default-src 'self'; " +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
                "style-src 'self' 'unsafe-inline'; " +
                "img-src 'self' data: https:; " +
                "font-src 'self' data:; " +
                "connect-src 'self' https:; " +
                "frame-ancestors 'none'");

            // Strict-Transport-Security: Enforces HTTPS
            if (context.Request.IsHttps)
            {
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
            }

            // Remove server header for security
            context.Response.Headers.Remove("Server");
            context.Response.Headers.Remove("X-Powered-By");

            await _next(context);
        }
    }
}

