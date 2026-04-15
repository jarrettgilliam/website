using System.IO;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Website.Endpoints;
using Website.Interfaces;
using Website.Middleware;
using Website.Models;
using Website.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<AppSecrets>()
    .BindConfiguration(nameof(AppSecrets))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddSingleton<IReCaptchaService, ReCaptchaService>()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddHttpClient()
    .AddResponseCompression()
    .AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        options.AddPolicy("api", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(1)
                    }));
    });

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCompression();
app.UseRateLimiter();
app.UseMiddleware<SecurityHeadersMiddleware>();

var fileServerOptions = new FileServerOptions
{
    StaticFileOptions =
    {
        OnPrepareResponse = ctx =>
        {
            ctx.Context.Response.Headers.CacheControl =
                ctx.File.Name is "index.html" or "robots.txt" or "favicon.ico" or "security.txt"
                    ? "public, max-age=3600" // Cannot use cache busting for these files, so cache for 1 hour
                    : "public, max-age=31536000, immutable"; // Cache all other files for 1 year
        }
    }
};

if (app.Environment.IsDevelopment() &&
    new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "..", "..", "web", "dist")) is { Exists: true } distInfo)
{
    fileServerOptions.FileProvider = new PhysicalFileProvider(distInfo.FullName);
}

app.UseFileServer(fileServerOptions);

app.MapResourceEndpoints();

app.Run();