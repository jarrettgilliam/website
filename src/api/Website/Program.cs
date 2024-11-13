using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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
    .AddResponseCompression();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCompression();
app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseFileServer(new FileServerOptions
{
    StaticFileOptions =
    {
        OnPrepareResponse = ctx =>
        {
            ctx.Context.Response.Headers.CacheControl = ctx.File.Name == "index.html"
                ? "public, max-age: 3600" // Cache index.html for 1 hour
                : "public, max-age: 31536000"; // Cache static files for 1 year
        }
    }
});

app.MapResourceEndpoints();

app.Run();