namespace Website.Middleware;

using Microsoft.AspNetCore.Http;

public sealed class SecurityHeadersMiddleware(RequestDelegate Next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Websites to test security headers:
        // https://securityheaders.com/
        // https://observatory.mozilla.org/
        // https://csp-evaluator.withgoogle.com/
        // https://report-uri.com/home/generate

        const string csp =
            "default-src 'self'; " +
            "script-src 'self' https://www.google.com https://www.gstatic.com; " +
            "style-src 'self' https://fonts.googleapis.com; " +
            "img-src 'self'; " +
            "font-src 'self' https://fonts.googleapis.com https://fonts.gstatic.com; " +
            "object-src 'none'; " +
            "frame-src 'self' https://www.google.com; " +
            "frame-ancestors 'self'; " +
            "base-uri 'self'; " +
            "upgrade-insecure-requests";

        context.Response.OnStarting(() =>
        {
            context.Response.Headers.XFrameOptions = "SAMEORIGIN";
            context.Response.Headers.XContentTypeOptions = "nosniff";
            context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            context.Response.Headers["Permissions-Policy"] = "camera=(), geolocation=(), microphone=()";
            context.Response.Headers.ContentSecurityPolicy = csp;
            return Task.CompletedTask;
        });

        await Next(context);
    }
}