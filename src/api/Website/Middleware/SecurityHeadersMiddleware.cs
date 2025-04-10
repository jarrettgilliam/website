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

        const string csp =
            "default-src 'none'; " +
            "script-src 'self' https://www.google.com https://www.gstatic.com https://plausible.jarrettgilliam.com; " +
            "style-src 'self'; " +
            "img-src 'self'; " +
            "font-src 'self'; " +
            "object-src 'none'; " +
            "frame-src 'self' https://www.google.com; " +
            "frame-ancestors 'self'; " +
            "base-uri 'self'; " +
            "connect-src 'self' https://plausible.jarrettgilliam.com/; " +
            "form-action 'none'; " +
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
