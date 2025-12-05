using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using Utils.Middleware;

namespace Utils.Extensions
{
    /// <summary>
    /// Extension methods for configuring security middleware
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Adds security headers middleware to the pipeline
        /// </summary>
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SecurityHeadersMiddleware>();
        }

        /// <summary>
        /// Adds rate limiting middleware to the pipeline
        /// </summary>
        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RateLimitingMiddleware>();
        }

        /// <summary>
        /// Adds request validation middleware to the pipeline
        /// </summary>
        public static IApplicationBuilder UseRequestValidation(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestValidationMiddleware>();
        }

        /// <summary>
        /// Adds all security middleware to the pipeline
        /// </summary>
        public static IApplicationBuilder UseApiSecurity(this IApplicationBuilder app)
        {
            app.UseSecurityHeaders();
            app.UseRequestValidation();
            app.UseRateLimiting();
            return app;
        }

        /// <summary>
        /// Configures rate limiting options
        /// </summary>
        public static IServiceCollection AddRateLimiting(
            this IServiceCollection services, 
            Action<RateLimitOptions> configure)
        {
            var options = new RateLimitOptions();
            configure(options);
            services.AddSingleton(options);
            
            // Add memory cache if not already registered
            if (!services.Any(x => x.ServiceType == typeof(IMemoryCache)))
            {
                services.AddMemoryCache();
            }
            
            return services;
        }

        /// <summary>
        /// Configures request validation options
        /// </summary>
        public static IServiceCollection AddRequestValidation(
            this IServiceCollection services,
            Action<RequestValidationOptions> configure)
        {
            var options = new RequestValidationOptions();
            configure(options);
            services.AddSingleton(options);
            return services;
        }
    }
}

