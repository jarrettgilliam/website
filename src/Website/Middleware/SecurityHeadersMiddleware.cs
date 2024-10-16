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

        const string nonce = "gWcA++mta0mZpLsdkEoEDw";

        const string csp =
            "default-src 'self'; " +
            $"script-src 'nonce-{nonce}' 'strict-dynamic' 'self' 'unsafe-inline' https://www.google.com https://www.gstatic.com https: http:; " +
            "style-src 'self' https://fonts.googleapis.com; " +
            "img-src 'self'; " +
            "font-src 'self' https://fonts.googleapis.com https://fonts.gstatic.com; " +
            "object-src 'none'; " +
            "frame-src 'self' https://www.google.com; " +
            "frame-ancestors 'self'; " +
            "base-uri 'self'; " +
            // "require-trusted-types-for 'script'; " +
            "upgrade-insecure-requests; " +
            $"script-src-attr 'nonce-{nonce}'";

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